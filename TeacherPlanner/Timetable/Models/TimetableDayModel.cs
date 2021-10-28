using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.DatabaseModels;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetableDayModel
    {
        public TimetableDayModel(List<TimetablePeriod> timetablePeriods, TimetableDisplayModes displayMode)
        {
            DisplayMode = displayMode;

            Registration = new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Registration1), displayMode);
            Period1 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period1), displayMode);
            Period2 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period2), displayMode);
            Break =        new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Break1), displayMode);
            Period3 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period3), displayMode);
            Lunch =        new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Lunch1), displayMode);
            Period4 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period4), displayMode);
            Period5 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period5), displayMode);
            Twilight =     new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Twilight), displayMode);
        }

        public TimetablePeriodModel Registration { get; }
        public TimetablePeriodModel Period1 { get; }
        public TimetablePeriodModel Period2 { get; }
        public TimetablePeriodModel Break { get; }
        public TimetablePeriodModel Period3 { get; }
        public TimetablePeriodModel Lunch { get; }
        public TimetablePeriodModel Period4 { get; }
        public TimetablePeriodModel Period5 { get; }
        public TimetablePeriodModel Twilight { get; }
        private TimetableDisplayModes DisplayMode { get; }
        public TimetablePeriodModel GetPeriod(PeriodCodes period)
        {
            return period switch
            {
                PeriodCodes.Registration1 => Registration,
                PeriodCodes.Period1 => Period1,
                PeriodCodes.Period2 => Period2,
                PeriodCodes.Break1 => Break,
                PeriodCodes.Period3 => Period3,
                PeriodCodes.Lunch1 => Lunch,
                PeriodCodes.Period4 => Period4,
                PeriodCodes.Period5 => Period5,
                PeriodCodes.Twilight => Twilight,
                _ => Registration,
            };
        }
    }
}
