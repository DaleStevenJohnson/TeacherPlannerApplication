using System;
using System.Collections.Generic;
using System.Linq;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetableModel
    {
        public TimetableModel()
        {
            Week1 = null;
            Week2 = null;
        }

        public void Update(List<TimetablePeriod> timetablePeriods)
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

        public void Filter(List<string> classCodes)
        {
            for (int week = 1; week <= 2; week++)
            {
                for (int day = (int)DayOfWeek.Monday; day < (int)DayOfWeek.Saturday; day++)
                {
                    foreach (PeriodCodes period in Enum.GetValues(typeof(PeriodCodes)))
                    {
                        var timetablePeriod = GetPeriod(week, day, period);
                        if (timetablePeriod.ClassCode != null)
                        {
                            if (!classCodes.Contains(timetablePeriod.ClassCode))
                            {
                                timetablePeriod.Clear();
                            }
                        }
                    }
                }
            }

        }

        public List<string> GetUniqueClassCodes()
        {
            var classcodes = new List<string>();
            for (int week = 1; week <= 2; week++)
            {
                for (int day = (int)DayOfWeek.Monday; day < (int)DayOfWeek.Saturday; day++)
                {
                    foreach(PeriodCodes period in Enum.GetValues(typeof(PeriodCodes)))
                    {
                        if (period == PeriodCodes.Registration)
                            break;
                        
                        var timetablePeriod = GetPeriod(week, day, period);
                        if (timetablePeriod.ClassCode != null)
                        {
                            if (!classcodes.Contains(timetablePeriod.ClassCode))
                                classcodes.Add(timetablePeriod.ClassCode);
                        }
                    }
                }
            }
            var myComparer = new CustomComparer();
            classcodes.Sort(myComparer);
            return classcodes;
        }
    }
}
