using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetableWeekModel
    {
        public TimetableWeekModel(string weekNumber)
        {
            WeekNumber = weekNumber;
            Monday = new TimetableDayModel();
            Tuesday = new TimetableDayModel();
            Wednesday = new TimetableDayModel();
            Thursday = new TimetableDayModel();
            Friday = new TimetableDayModel();
        }
        public string WeekNumber { get; }
        public TimetableDayModel Monday { get; }
        public TimetableDayModel Tuesday { get; }
        public TimetableDayModel Wednesday { get; }
        public TimetableDayModel Thursday { get; }
        public TimetableDayModel Friday { get; }
        public TimetablePeriodModel GetPeriod(int day, string period)
        {
            return day switch
            {
                1 => Monday.GetPeriod(period),
                2 => Tuesday.GetPeriod(period),
                3 => Wednesday.GetPeriod(period),
                4 => Thursday.GetPeriod(period),
                5 => Friday.GetPeriod(period),
                _ => Monday.GetPeriod(period),
            };
        }
    }
}
