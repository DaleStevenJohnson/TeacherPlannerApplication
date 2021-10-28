using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeacherPlanner.Constants
{
    public enum PeriodCodes
    {
        Undefined = 0,
        Period1 = 1,
        Period2 = 2,
        Period3 = 3,
        Period4 = 4,
        Period5 = 5,
        Period6 = 6,
        Period7 = 7,
        Period8 = 8,
        Period9 = 9,
        Period10 = 10,

        Pastoral = 100,
        Registration1 = 101,
        Registration2 = 102,

        Breaks = 200,
        Break1 = 201,
        Break2 = 202,

        Lunches = 300,
        Lunch1 = 301,
        Lunch2 = 302,

        AfterSchool = 400,
        Twilight = 401,
    }

    public static class PeriodCodesConverter
    {
        public static PeriodCodes ConvertIntToPeriodCodes(int period)
        {
            return period switch
            {
                0 => PeriodCodes.Registration1,
                1 => PeriodCodes.Period1,
                2 => PeriodCodes.Period2,
                3 => PeriodCodes.Break1,
                4 => PeriodCodes.Period3,
                5 => PeriodCodes.Lunch1,
                6 => PeriodCodes.Period4,
                7 => PeriodCodes.Period5,
                8 => PeriodCodes.Twilight,
                _ => PeriodCodes.Registration1,
            };
        }

        public static PeriodCodes ConvertPeriodTypeToPeriodCodes(string periodType)
        {
            switch (periodType)
            {
                case "Registration":
                    return PeriodCodes.Registration1;
                case "Lesson":
                    return PeriodCodes.Period1;
                case "Break":
                    return PeriodCodes.Break1;
                case "Lunch":
                    return PeriodCodes.Lunch1;
                case "Twilight":
                    return PeriodCodes.Twilight;
                default:
                    return PeriodCodes.Undefined;
            }
        }
    }

    public static class Enums
    {
        public static T Next<T>(this T v) where T : struct
        {
            return Enum.GetValues(v.GetType()).Cast<T>().Concat(new[] { default(T) }).SkipWhile(e => !v.Equals(e)).Skip(1).First();
        }

        public static T Previous<T>(this T v) where T : struct
        {
            return Enum.GetValues(v.GetType()).Cast<T>().Concat(new[] { default(T) }).Reverse().SkipWhile(e => !v.Equals(e)).Skip(1).First();
        }
    }

}
