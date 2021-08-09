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
            
            NoteSection = new NoteSectionModel(date);
            PopulatePeriods();
        }

        
        public PeriodModel Period1 { get; set; }
        public PeriodModel Period2 { get; set; }
        public PeriodModel Period3 { get; set; }
        public PeriodModel Period4 { get; set; }
        public PeriodModel Period5 { get; set; }
        public PeriodModel Period6 { get; set; }

        public NoteSectionModel NoteSection { get; set; }
        public PageViewModel MyParent { get; }


        

        

        private void PopulatePeriods() 
        {
            string[] forms = new string[6];
            Random random = new Random();
            for (int i = 0; i < forms.Length; i++)
            {

                int year = random.Next(7, 13);
                string[] formcodes = new string[] { "A", "J", "P", "C" };
                int r = random.Next(0, 3);
                forms[i] = $"{year}{formcodes[r]}";
            }
            Period1 = new PeriodModel(1, forms[0], NoteSection.Calendar.DisplayDate);
            Period2 = new PeriodModel(2, forms[1], NoteSection.Calendar.DisplayDate);
            Period3 = new PeriodModel(3, forms[2], NoteSection.Calendar.DisplayDate);
            Period4 = new PeriodModel(4, forms[3], NoteSection.Calendar.DisplayDate);
            Period5 = new PeriodModel(5, forms[4], NoteSection.Calendar.DisplayDate);
            Period6 = new PeriodModel(6, forms[5], NoteSection.Calendar.DisplayDate);

        }
    }
}
