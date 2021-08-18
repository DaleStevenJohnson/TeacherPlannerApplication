using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetablePeriodModel
    {
        public TimetablePeriodModel()
        {
            
        }

        public string ClassCode { get; set; }
        public string Room { get; set; }
        public string Occurance { get; set; }
        public string Occurances { get; set; }
    }
}
