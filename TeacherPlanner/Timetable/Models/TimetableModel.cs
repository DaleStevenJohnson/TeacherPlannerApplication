using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.DatabaseModels;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetableModel
    {
        public TimetableModel(List<TimetablePeriod> timetablePeriods)
        {
            Week1 = new TimetableWeekModel(timetablePeriods.Where(t => t.Week == 1).ToList(), 1);
            Week2 = new TimetableWeekModel(timetablePeriods.Where(t => t.Week == 2).ToList(), 2);
        }

        // Properties
        
        public TimetableWeekModel Week1 { get; private set; }
        public TimetableWeekModel Week2 { get; private set; }

        // Public Methods
        public TimetablePeriodModel GetPeriod(int week, int day, PeriodCodes period)
        {
            return week == 1 ? Week1.GetPeriod(day, period) : Week2.GetPeriod(day, period);
        }
    }
}
