using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TeacherPlanner.Planner.Models
{
    public class YearSelectModel
    {
        public YearSelectModel(int year)
        {
            Year = year;
            int nextYear = year + 1;
            AcademicYear = $"{year} - {nextYear}";
        }
        public string AcademicYear { get; }
        public int Year { get; }
    }
}
