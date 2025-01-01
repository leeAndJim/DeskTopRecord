using System.IO;
using System.Windows;
using System.Windows.Input;

namespace DeskTopRecord
{
    public partial class NoteWindow : Window
    {
        private const string NotesDirectory = @"D:\study\计算机\AI编程学习\便签\DeskTopRecord\DeskTopRecord\DeskTopRecord\data";
        public event EventHandler NoteSaved;
        private string originalFileName;

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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string noteName = NoteNameTextBox.Text;
            string noteContent = NoteTextBox.Text;

            if (string.IsNullOrWhiteSpace(noteName))
            {
                MessageBox.Show("Please enter a note name.");
                return;
            }

            string newFilePath = Path.Combine(NotesDirectory, $"{noteName}.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));
            File.WriteAllText(newFilePath, noteContent);
            MessageBox.Show("Note saved successfully!");

            if (!string.IsNullOrEmpty(originalFileName) && originalFileName != newFilePath)
            {
                File.Delete(originalFileName);
            }
            NoteSaved?.Invoke(this, EventArgs.Empty);
        }
        public void SetOriginalFileName(string fileName)
        {
            originalFileName = fileName;
        }
        private void NoteNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }

        private void UpdatePlaceholderVisibility()
        {
            NoteNamePlaceholder.Visibility = string.IsNullOrEmpty(NoteNameTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
