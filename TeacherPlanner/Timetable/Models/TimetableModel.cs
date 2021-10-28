using System;
using System.Collections.Generic;
using System.Linq;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetableModel : ObservableObject
    {
        private TimetableWeekModel _week1;
        private TimetableWeekModel _week2;

        public TimetableModel(TimetableDisplayModes displayMode)
        {
            Week1 = null;
            Week2 = null;

            DisplayMode = displayMode;
        }

        public void Update(List<TimetablePeriod> timetablePeriods)
        {
            Week1 = new TimetableWeekModel(timetablePeriods.Where(t => t.Week == 1).ToList(), 1, DisplayMode);
            Week2 = new TimetableWeekModel(timetablePeriods.Where(t => t.Week == 2).ToList(), 2, DisplayMode);
        }

        // Properties

        public TimetableWeekModel Week1 
        {
            get => _week1;
            private set => RaiseAndSetIfChanged(ref _week1, value);
        }
        public TimetableWeekModel Week2
        {
            get => _week2;
            private set => RaiseAndSetIfChanged(ref _week2, value);
        }

        public TimetableDisplayModes DisplayMode { get; set; }

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

            if (Week1 != null && Week2 != null)
            {
                for (int week = 1; week <= 2; week++)
                {
                    for (int day = (int)DayOfWeek.Monday; day < (int)DayOfWeek.Saturday; day++)
                    {
                        foreach (PeriodCodes period in Enum.GetValues(typeof(PeriodCodes)))
                        {
                            if (period == PeriodCodes.Registration1)
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
                var myComparer = new ClassCodeComparer();
                classcodes.Sort(myComparer);
            }
            return classcodes;
        }
    }
}
