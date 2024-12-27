using System.Windows;
using System.Windows.Input;

namespace DeskTopRecord
{
    public partial class NoteWindow : Window
    {
        public NoteWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            ZoomIn();
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            ZoomOut();
        }

        public void ZoomIn()
        {
            this.Width += 20;
            this.Height += 20;
        }

        public void ZoomOut()
        {
            this.Width = this.Width > 40 ? this.Width - 20 : this.Width;
            this.Height = this.Height > 40 ? this.Height - 20 : this.Height;
        }
    }
}
