using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;

namespace DeskTopRecord
{
    public partial class MainWindow : Window
    {
        private NoteManager noteManager;
        private List<NoteWindow> noteWindows;

        public MainWindow()
        {
            InitializeComponent();
            noteManager = new NoteManager();
            noteWindows = new List<NoteWindow>();

            LoadNotes();
            RegisterHotKeys();
        }

        private void CreateNote_Click(object sender, RoutedEventArgs e)
        {
            NoteWindow noteWindow = new NoteWindow();
            noteWindow.Closed += NoteWindow_Closed;
            noteWindow.Show();
            noteWindows.Add(noteWindow);
        }

        private void NoteWindow_Closed(object sender, System.EventArgs e)
        {
            NoteWindow noteWindow = sender as NoteWindow;
            if (noteWindow != null)
            {
                noteWindows.Remove(noteWindow);
            }
        }

        private void LoadNotes()
        {
            List<string> notes = noteManager.LoadNotes();
            foreach (string note in notes)
            {
                NoteWindow noteWindow = new NoteWindow();
                noteWindow.NoteTextBox.Text = note;
                noteWindow.Closed += NoteWindow_Closed;
                noteWindow.Show();
                noteWindows.Add(noteWindow);
            }
        }

        private void SaveNotes()
        {
            List<string> notes = new List<string>();
            foreach (NoteWindow noteWindow in noteWindows)
            {
                notes.Add(noteWindow.NoteTextBox.Text);
            }
            noteManager.SaveNotes(notes);
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveNotes();
            base.OnClosed(e);
        }

        private void RegisterHotKeys()
        {
            HotkeyManager.Current.AddOrReplace("CreateNoteCtrlN", Key.N, ModifierKeys.Control, OnCreateNoteHotkey);
            HotkeyManager.Current.AddOrReplace("CreateNoteCtrlF2", Key.F2, ModifierKeys.Control, OnCreateNoteHotkey);
            HotkeyManager.Current.AddOrReplace("DestroyNoteCtrlD", Key.D, ModifierKeys.Control, OnDestroyNoteHotkey);
            HotkeyManager.Current.AddOrReplace("ZoomInCtrlPlus", Key.OemPlus, ModifierKeys.Control, OnZoomInHotkey);
            HotkeyManager.Current.AddOrReplace("ZoomOutCtrlMinus", Key.OemMinus, ModifierKeys.Control, OnZoomOutHotkey);
        }

        private void OnCreateNoteHotkey(object sender, HotkeyEventArgs e)
        {
            CreateNote_Click(sender, null);
            e.Handled = true;
        }

        private void OnDestroyNoteHotkey(object sender, HotkeyEventArgs e)
        {
            if (noteWindows.Count > 0)
            {
                NoteWindow noteWindow = noteWindows[noteWindows.Count - 1];
                noteWindow.Close();
            }
            e.Handled = true;
        }

        private void OnZoomInHotkey(object sender, HotkeyEventArgs e)
        {
            if (noteWindows.Count > 0)
            {
                NoteWindow noteWindow = noteWindows[noteWindows.Count - 1];
                noteWindow.ZoomIn();
            }
            e.Handled = true;
        }

        private void OnZoomOutHotkey(object sender, HotkeyEventArgs e)
        {
            if (noteWindows.Count > 0)
            {
                NoteWindow noteWindow = noteWindows[noteWindows.Count - 1];
                noteWindow.ZoomOut();
            }
            e.Handled = true;
        }
    }
}
