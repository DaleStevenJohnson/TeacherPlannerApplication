using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Planner.Models
{
    public class CalendarDateModel
    {
        //public event EventHandler<string> CalendarDayClickedEvent;
        public CalendarDateModel(string dayDate, int week)
        {
            DayDate = dayDate;
            Week = week;
        }
        public string DayDate { get; }
        public int Week { get; }
        public bool IsDisplayedDate { get; set; }
    }
}
