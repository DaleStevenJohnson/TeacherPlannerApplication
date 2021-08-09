using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Models
{
    public class NoteSectionModel
    {
        public NoteSectionModel(DateTime date) 
        {
            Calendar = new CalendarModel(date);
            PopulateNotes();
            
        }

        public CalendarModel Calendar { get; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string Note4 { get; set; }
        public string Note5 { get; set; }
        public string Note6 { get; set; }

        private void PopulateNotes()
        {
            string[] words = new string[] { "This", "is", "random", "text", "hello", "Goodbye", "and", "just", "is", "not", "could", "be", "Marjorie" };
            Random r = new Random();

            Note1 = $"{words[r.Next(0, words.Length)]}";
            Note2 = $"{words[r.Next(0, words.Length)]}";
            Note3 = $"{words[r.Next(0, words.Length)]}";
            Note4 = $"{words[r.Next(0, words.Length)]}";
            Note5 = $"{words[r.Next(0, words.Length)]}";
            Note6 = $"{words[r.Next(0, words.Length)]}";
        }
    }
}
