using System;
using System.Collections.Generic;
using System.Text;
using Database.DatabaseModels;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetablePeriodModel
    {
        public TimetablePeriodModel(TimetablePeriod timetablePeriod)
        {
            if (timetablePeriod != null)
            {
                ID = timetablePeriod.ID;
                ClassCode = timetablePeriod.ClassCode;
                Room = timetablePeriod.RoomCode;
            }
        }
        public int ID { get; }
        public string ClassCode { get; set; }
        public string Room { get; set; }
        public int Occurance { get; set; }
        public int Occurances { get; set; }
    }
}
