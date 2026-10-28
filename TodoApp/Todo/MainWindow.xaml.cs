using Microsoft.Win32;
using System;
using System.Windows;

namespace TodoApp
{
    public partial class MainWindow : Window
    {
        private readonly TodoService _todoService;
        private readonly string _defaultFilePath = "tasks.json";

        public MainWindow()
        {
            InitializeComponent();
            _todoService = new TodoService(new JsonTaskRepository());
            DataGridTasks.ItemsSource = _todoService.Tasks;

            CmbPriority.ItemsSource = Enum.GetValues(typeof(TaskPriority));
            CmbStatus.ItemsSource = Enum.GetValues(typeof(TaskStatus));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _todoService.LoadTasks(_defaultFilePath);
            }
            catch
            {
                
            }
        }

        private void DataGridTasks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DataGridTasks.SelectedItem is TodoModel selectedTask)
            {
                TxtTitle.Text = selectedTask.Title;
                TxtDescription.Text = selectedTask.Description;
                DpDueDate.SelectedDate = selectedTask.DueDate;
                CmbPriority.SelectedItem = selectedTask.Priority;
                CmbStatus.SelectedItem = selectedTask.Status;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var task = new TodoModel
                {
                    Title = TxtTitle.Text,
                    Description = TxtDescription.Text,
                    CreatedAt = DateTime.Now,
                    DueDate = DpDueDate.SelectedDate ?? DateTime.Now.AddDays(1),
                    Priority = CmbPriority.SelectedItem is TaskPriority p ? p : TaskPriority.Medium,
                    Status = CmbStatus.SelectedItem is TaskStatus s ? s : TaskStatus.NotStarted
                };

                _todoService.AddTask(task);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridTasks.SelectedItem is TodoModel selectedTask)
            {
                try
                {
                    selectedTask.Title = TxtTitle.Text;
                    selectedTask.Description = TxtDescription.Text;
                    if (DpDueDate.SelectedDate.HasValue)
                        selectedTask.DueDate = DpDueDate.SelectedDate.Value;
                    if (CmbPriority.SelectedItem is TaskPriority p)
                        selectedTask.Priority = p;
                    if (CmbStatus.SelectedItem is TaskStatus s)
                        selectedTask.Status = s;

                    DataGridTasks.Items.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridTasks.SelectedItem is TodoModel selectedTask)
            {
                _todoService.RemoveTask(selectedTask);
                ClearForm();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "JSON files (*.json)|*.json", FileName = "tasks.json" };
            if (dialog.ShowDialog() == true)
            {
                _todoService.SaveTasks(dialog.FileName);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "JSON files (*.json)|*.json" };
            if (dialog.ShowDialog() == true)
            {
                _todoService.LoadTasks(dialog.FileName);
            }
        }

        private void ClearForm()
        {
            TxtTitle.Clear();
            TxtDescription.Clear();
            DpDueDate.SelectedDate = null;
            CmbPriority.SelectedIndex = -1;
            CmbStatus.SelectedIndex = -1;
            DataGridTasks.SelectedItem = null;
        }
    }
}