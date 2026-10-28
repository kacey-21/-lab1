using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoApp;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<TodoItem> _items = new();
    private string _filter = "all";

    public MainWindow()
    {
        InitializeComponent();
        List.ItemsSource = _items;
    }

    private void Add_Click(object sender, RoutedEventArgs e) => AddFromInput();

    private void NewText_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) AddFromInput();
    }

    private void AddFromInput()
    {
        var text = NewText.Text.Trim();
        if (text.Length == 0) return;
        _items.Add(new TodoItem { Text = text });
        NewText.Clear();
        NewText.Focus();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: TodoItem item })
            _items.Remove(item);
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: TodoItem item }) return;

        var dialog = new Window
        {
            Title = "Редактировать",
            Width = 400,
            Height = 120,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Owner = this,
            ResizeMode = ResizeMode.NoResize
        };

        var grid = new Grid { Margin = new Thickness(12) };
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var box = new TextBox { Text = item.Text, Padding = new Thickness(6) };
        Grid.SetRow(box, 0);
        grid.Children.Add(box);

        var ok = new Button
        {
            Content = "OK",
            Width = 80,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 8, 0, 0)
        };
        Grid.SetRow(ok, 1);
        ok.Click += (_, _) =>
        {
            var t = box.Text.Trim();
            if (t.Length > 0) item.Text = t;
            dialog.Close();
        };
        grid.Children.Add(ok);

        dialog.Content = grid;
        dialog.ShowDialog();
    }

    private void Filter_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: string f })
        {
            _filter = f;
            RefreshList();
        }
    }

    private void RefreshList()
    {
        // Простая «фильтрация пересборкой» — меньше магии, чем ICollectionView.Filter
        IEnumerable<TodoItem> view = _filter switch
        {
            "active" => _items.Where(i => !i.Done),
            "done"   => _items.Where(i => i.Done),
            _        => _items
        };
        List.ItemsSource = view.ToList();
    }
}