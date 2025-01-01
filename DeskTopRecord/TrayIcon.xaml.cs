// TrayIcon.xaml.cs
using System.Windows;

namespace DeskTopRecord
{
    public partial class TrayIcon
    {
        public TrayIcon()
        {
            InitializeComponent();
        }

        private void ShowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Activate();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
