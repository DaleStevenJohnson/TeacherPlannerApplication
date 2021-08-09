using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Models
{
    public class NoteSectionModel
    {
        public NoteSectionModel(DateTime date)
        {
            Notes = new string[6];
            Calendar = new CalendarModel(date);
        }
        public string[] Notes { get; set; }

        public CalendarModel Calendar { get; }
        public string Note1
        {
            get { return Notes[0]; }
            set => Notes[0] = value;
        }
        public string Note2
        {
            get { return Notes[1]; }
            set => Notes[1] = value;
        }
        public string Note3
        {
            get { return Notes[2]; }
            set => Notes[2] = value;
        }
        public string Note4
        {
            get { return Notes[3]; }
            set => Notes[3] = value;
        }
        public string Note5
        {
            get { return Notes[4]; }
            set => Notes[4] = value;
        }
        public string Note6
        {
            get { return Notes[5]; }
            set => Notes[5] = value;
        }

        public void Load(string[] notes)
        {
            for (int i = 0; i < notes.Length; i++)
            {
                Notes[i] = notes[i];
            }
        }

        private void LoadTestNotes()
        {
            string[] words = new string[] { "this", "is", "random", "text", "hello", "goodbye", "and", "just", "is", "not", "could", "be", "Marjorie", "angry", "hornets" };
            Random r = new Random();

            for (int i = 0; i < Notes.Length; i++)
            {
                int sentenceLength = r.Next(3, 7);
                string sentence = "";
                for (int j = 0; j < sentenceLength; j++)
                {
                    int wordIndex = r.Next(0, words.Length);
                    string word = words[wordIndex];
                    sentence += word;
                    if (j != sentenceLength - 1)
                        sentence += " ";
                }
                Notes[i] = sentence;
            }
        }

        internal string PackageSaveData()
        {
            string saveData = "";
            string[] cleanNotes = FileHandlingHelper.SanitiseStrings(Notes);
            for (int i = 0; i < cleanNotes.Length; i++)
            {
                saveData += $"\n{Notes[i]}";
            }
            return saveData;
        }
    }
}
