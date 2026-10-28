using System.Windows;
using System.Windows.Threading;

namespace TodoApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        DispatcherUnhandledException += (_, args) =>
        {
            MessageBox.Show(args.Exception.ToString(), "Ошибка");
            args.Handled = true;
        };
        base.OnStartup(e);
    }
}