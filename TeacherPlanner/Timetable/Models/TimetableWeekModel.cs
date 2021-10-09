using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.DatabaseModels;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetableWeekModel
    {
        public TimetableWeekModel(List<TimetablePeriod> timetablePeriods, int week)
        {
            
            WeekNumber = week;
            Monday = new TimetableDayModel(timetablePeriods.Where(t => t.Day == (int)DayOfWeek.Monday).ToList());
            Tuesday = new TimetableDayModel(timetablePeriods.Where(t => t.Day == (int)DayOfWeek.Tuesday).ToList());
            Wednesday = new TimetableDayModel(timetablePeriods.Where(t => t.Day == (int)DayOfWeek.Wednesday).ToList());
            Thursday = new TimetableDayModel(timetablePeriods.Where(t => t.Day == (int)DayOfWeek.Thursday).ToList());
            Friday = new TimetableDayModel(timetablePeriods.Where(t => t.Day == (int)DayOfWeek.Friday).ToList());
        }
        public int WeekNumber { get; }
        public TimetableDayModel Monday { get; }
        public TimetableDayModel Tuesday { get; }
        public TimetableDayModel Wednesday { get; }
        public TimetableDayModel Thursday { get; }
        public TimetableDayModel Friday { get; }
        public TimetablePeriodModel GetPeriod(int day, PeriodCodes period)
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
