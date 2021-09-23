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
        public TimetableDayModel(List<TimetablePeriod> timetablePeriods)
        {
            Registration = new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Registration));
            Period1 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period1));
            Period2 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period2));
            Break =        new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Break));
            Period3 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period3));
            Lunch =        new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Lunch));
            Period4 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period4));
            Period5 =      new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Period5));
            Twilight =     new TimetablePeriodModel(timetablePeriods.FirstOrDefault(t => t.Period == (int)PeriodCodes.Twilight));
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
        public TimetablePeriodModel GetPeriod(PeriodCodes period)
        {
            return period switch
            {
                PeriodCodes.Registration => Registration,
                PeriodCodes.Period1 => Period1,
                PeriodCodes.Period2 => Period2,
                PeriodCodes.Break => Break,
                PeriodCodes.Period3 => Period3,
                PeriodCodes.Lunch => Lunch,
                PeriodCodes.Period4 => Period4,
                PeriodCodes.Period5 => Period5,
                PeriodCodes.Twilight => Twilight,
                _ => Registration,
            };
        }
    }
}
