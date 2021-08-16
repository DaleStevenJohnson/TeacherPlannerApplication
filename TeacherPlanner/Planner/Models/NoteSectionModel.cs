using System;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.Models
{
    public class NoteSectionModel
    {
        public NoteSectionModel()
        {
            Notes = new string[6];
            //Calendar = new CalendarModel(date);
        }
        public string[] Notes { get; set; }

        //public CalendarModel Calendar { get; }
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

        internal string PackageSaveData()
        {
            string saveData = string.Empty;
            string[] cleanNotes = FileHandlingHelper.SanitiseStrings(Notes);
            for (int i = 0; i < cleanNotes.Length; i++)
            {
                saveData += $"\n{Notes[i]}";
            }
            return saveData;
        }
    }
}
