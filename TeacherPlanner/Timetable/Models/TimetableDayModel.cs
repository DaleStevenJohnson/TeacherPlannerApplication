using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetableDayModel
    {
        public TimetableDayModel()
        {
            Registration = new TimetablePeriodModel();
            Period1 = new TimetablePeriodModel();
            Period2 = new TimetablePeriodModel();
            Break = new TimetablePeriodModel();
            Period3 = new TimetablePeriodModel();
            Lunch = new TimetablePeriodModel();
            Period4 = new TimetablePeriodModel();
            Period5 = new TimetablePeriodModel();
            Twilight = new TimetablePeriodModel();
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
        public TimetablePeriodModel GetPeriod(string period)
        {
            return period switch
            {
                "R" => Registration,
                "1" => Period1,
                "2" => Period2,
                "B" => Break,
                "3" => Period3,
                "L" => Lunch,
                "4" => Period4,
                "5" => Period5,
                "T" => Twilight,
                _ => Registration,
            };
        }
    }
}
