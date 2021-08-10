using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Helpers;
using System.Windows.Input;
using TeacherPlanner.ViewModels;

namespace TeacherPlanner.Models
{
    public class Day : ObservableObject
    {

        public Day(DateTime date, PageViewModel parent)
        {
            MyParent = parent;
            Periods = new PeriodModel[6];
            NoteSection = new NoteSectionModel(date);
        }

        private PeriodModel[] Periods;
        public PeriodModel Period1 
        {
            get { return Periods[0]; }
            set => Periods[0] = value; 
        }
        public PeriodModel Period2
        {
            get { return Periods[1]; }
            set => Periods[1] = value;
        }
        public PeriodModel Period3
        {
            get { return Periods[2]; }
            set => Periods[2] = value;
        }
        public PeriodModel Period4
        {
            get { return Periods[3]; }
            set => Periods[3] = value;
        }
        public PeriodModel Period5
        {
            get { return Periods[4]; }
            set => Periods[4] = value;
        }
        public PeriodModel Period6
        {
            get { return Periods[5]; }
            set => Periods[5] = value;
        }

        public NoteSectionModel NoteSection { get; set; }
        public PageViewModel MyParent { get; }
        public bool TryLoad(string username)//, out string[] data)
        {
            string[] data = FileHandlingHelper.LoadDataFromFile(username, NoteSection.Calendar.FileNameDate);
            int periodRows = 7;
            int jump = 8;
            int position = 0;

            if (data.Length > 0)
            {
                for (int i = 0; i < Periods.Length; i++)
                {
                    string[] periodData = new string[periodRows];
                    int classCodeIndex = i * jump;
                    int startIndex = classCodeIndex + 1;
                    int periodIndex = 0;
                    for (int j = startIndex; j < startIndex + periodRows; j++)
                    {
                        periodData[periodIndex] = data[j];
                        periodIndex++;
                    }
                    
                    Periods[i] = new PeriodModel(i + 1, data[classCodeIndex], NoteSection.Calendar.DisplayDate);
                    Periods[i].Load(periodData);
                    position = classCodeIndex + periodRows;
                }
                int noteRows = 6;
                string[] noteData = new string[noteRows];
                int noteIndex = 0;
                for (int j = position+1; j < position + 1 + noteRows; j++)
                {
                    noteData[noteIndex] = data[j];
                    noteIndex++;
                }
                NoteSection.Load(noteData);
            }
            else
                LoadEmpty();
            return true;
        }

        private bool LoadEmpty()
        {
            for (int i = 0; i < Periods.Length; i++)
            {
                Periods[i] = new PeriodModel(i + 1, "", NoteSection.Calendar.DisplayDate);
                Periods[i].Load(new string[] { " ` ` ", " ` ` " , " ` ` " , " ` ` " , " ` ` " , " ` ` " , " ` ` " });
            }
            NoteSection.Load(new string[] { " ", " ", " ", " ", " ", " " });
            return true;
        }

        public bool Save(string username)
        {
            string data = PackageSaveData();
            FileHandlingHelper.Overwrite(username, NoteSection.Calendar.FileNameDate, data);
            return true;
        }

        private string PackageSaveData()
        {
            string saveData = "";

            for (int i = 0; i < Periods.Length; i++)
            {
                saveData += Periods[i].PackageSaveData();
                if (i != Periods.Length - 1) saveData += "\n";
            }
            string notes = NoteSection.PackageSaveData();
            saveData += notes;
            
            return saveData;
        }
    }
}
