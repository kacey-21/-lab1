using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoApp;

public class TodoItem : INotifyPropertyChanged
{
    private string _text = "";
    private bool _done;

    public string Text
    {
        get => _text;
        set { _text = value; OnPropertyChanged(); }
    }

    public bool Done
    {
        get => _done;
        set { _done = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}