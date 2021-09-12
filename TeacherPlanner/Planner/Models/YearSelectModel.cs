using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TeacherPlanner.Planner.Models
{
    public class YearSelectModel
    {
        public YearSelectModel(string year)
        {
            Year = year;
            int thisYear = Int32.Parse(year);
            int nextYear = thisYear + 1;
            AcademicYear = thisYear.ToString() + " - " + nextYear.ToString();
        }
        public string AcademicYear { get; }
        private string Year { get; }
    }
}
