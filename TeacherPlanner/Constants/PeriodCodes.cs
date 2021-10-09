using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Constants
{
    public enum PeriodCodes
    {

        Period1 = 1,
        Period2 = 2,
        Period3 = 3,
        Period4 = 4,
        Period5 = 5,

        Registration = 100,
        Break = 101,
        Lunch = 102,
        Twilight = 103,
    }

    public static class PeriodCodesConverter
    {
        public static PeriodCodes ConvertIntToPeriodCodes(int period)
        {
            return period switch
            {
                0 => PeriodCodes.Registration,
                1 => PeriodCodes.Period1,
                2 => PeriodCodes.Period2,
                3 => PeriodCodes.Break,
                4 => PeriodCodes.Period3,
                5 => PeriodCodes.Lunch,
                6 => PeriodCodes.Period4,
                7 => PeriodCodes.Period5,
                8 => PeriodCodes.Twilight,
                _ => PeriodCodes.Registration,
            };
        }
    }

}
