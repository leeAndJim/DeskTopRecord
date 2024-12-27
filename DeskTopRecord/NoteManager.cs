using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DeskTopRecord
{
    public class NoteManager
    {
        private const string NotesFilePath = "notes.json";

        public List<string> LoadNotes()
        {
            if (File.Exists(NotesFilePath))
            {
                string json = File.ReadAllText(NotesFilePath);
                return JsonSerializer.Deserialize<List<string>>(json);
            }
            return new List<string>();
        }

        public void SaveNotes(List<string> notes)
        {
            string json = JsonSerializer.Serialize(notes);
            File.WriteAllText(NotesFilePath, json);
        }
    }
}
