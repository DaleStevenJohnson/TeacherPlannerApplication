using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TeacherPlanner.Planner.Models
{
    public class AcademicYearModel
    {
        public AcademicYearModel(int year, int id)
        {
            ID = id;
            Year = year;
            int nextYear = year + 1;
            AcademicYear = $"{year} - {nextYear}";
        }
        public int ID { get; }
        public string AcademicYear { get; }
        public int Year { get; }
    }
}
