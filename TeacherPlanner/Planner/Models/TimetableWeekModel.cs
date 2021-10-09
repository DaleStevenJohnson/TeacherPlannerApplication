using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Planner.Models
{
    public class TimetableWeekModel
    {
        public TimetableWeekModel(int id, DateTime date, int week = 0)
        {
            ID = id;
            Date = date;
            if (Enum.IsDefined(typeof(Weeks), week))
                Week = (Weeks)week;
            else
                Week = Weeks.Unset;
        }
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public Weeks Week { get; set; }
    }
}
