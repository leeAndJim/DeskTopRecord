﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;

namespace DeskTopRecord
{
    public partial class MainWindow : Window
    {
        private const string NotesDirectory = @"D:\study\计算机\AI编程学习\便签\DeskTopRecord\DeskTopRecord\DeskTopRecord\data";
        private NoteManager noteManager;
        private List<NoteWindow> noteWindows;
        private TrayIcon trayIcon;

        private void OpenSavedNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem != null)
            {
                dynamic selectedNote = NotesListBox.SelectedItem;
                string filePath = Path.Combine(NotesDirectory, $"{selectedNote.Name}.txt");
                if (File.Exists(filePath))
                {
                    string noteContent = File.ReadAllText(filePath);
                    NoteWindow noteWindow = new NoteWindow();
                    noteWindow.NoteNameTextBox.Text = selectedNote.Name;
                    noteWindow.NoteTextBox.Text = noteContent;
                    noteWindow.SetOriginalFileName(filePath); // 设置原来的文件名
                    noteWindow.NoteSaved += NoteWindow_NoteSaved; // 订阅 NoteSaved 事件
                    noteWindow.Closed += NoteWindow_Closed;
                    noteWindow.Show();
                    noteWindows.Add(noteWindow);
                }
                else
                {
                    MessageBox.Show("No saved note found.");
                }
            }
            else
            {
                MessageBox.Show("Please select a note to open.");
            }
        }

        private void NotesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Handle selection change if needed
        }

        private void LoadNotes()
        {
            NotesListBox.Items.Clear();
            if (Directory.Exists(NotesDirectory))
            {
                var noteFiles = Directory.GetFiles(NotesDirectory, "*.txt");
                foreach (var file in noteFiles)
                {
                    string noteContent = File.ReadAllText(file);
                    string firstLine = noteContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                    NotesListBox.Items.Add(new { Name = Path.GetFileNameWithoutExtension(file), FirstLine = firstLine });
                }
            }
        }



        public MainWindow()
        {
            InitializeComponent();
            noteManager = new NoteManager();
            noteWindows = new List<NoteWindow>();

            LoadNotes();
            RegisterHotKeys();
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            trayIcon = new TrayIcon();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("关闭应用还是隐藏到托盘？", "确认", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SaveNotes();
                Application.Current.Shutdown();
            }
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // 取消关闭事件
                this.Hide();
            }
            else
            {
                e.Cancel = true; // 取消关闭事件
            }
        }

        private void CreateNote_Click(object sender, RoutedEventArgs e)
        {
            NoteWindow noteWindow = new NoteWindow();
            noteWindow.NoteSaved += NoteWindow_NoteSaved;
            noteWindow.Closed += NoteWindow_Closed;
            noteWindow.Show();
            noteWindows.Add(noteWindow);
        }
        private void NoteWindow_NoteSaved(object sender, EventArgs e)
        {
            LoadNotes();
        }

        private void NoteWindow_Closed(object sender, EventArgs e)
        {
            NoteWindow noteWindow = sender as NoteWindow;
            if (noteWindow != null)
            {
                noteWindows.Remove(noteWindow);
            }
        }

        /*private void LoadNotes()
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
        }*/

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

        private void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem != null)
            {
                dynamic selectedNote = NotesListBox.SelectedItem;
                string filePath = Path.Combine(NotesDirectory, $"{selectedNote.Name}.txt");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    LoadNotes();
                    MessageBox.Show("Note deleted successfully!");
                }
                else
                {
                    MessageBox.Show("No saved note found.");
                }
            }
            else
            {
                MessageBox.Show("Please select a note to delete.");
            }
        }
    }
}
