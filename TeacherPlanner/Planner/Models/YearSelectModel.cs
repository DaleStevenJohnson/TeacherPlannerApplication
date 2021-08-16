using System;
using System.Collections.Generic;
using System.Text;

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
        private string Year { get; }
        public string AcademicYear { get; }
    }
}
