using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Constants
{
    public static class Formats
    {
        public static string DateHeadingFormat { get; } = "dddd, dd MMMM";
        public static string FullDateFormat { get; } = "yyyyMMdd";
        public static string FullDayNameFormat { get; } = "dddd";
        public static string FullMonthNameFormat { get; } = "MMMM";
        public static string FullMonthNumberFormat { get; } = "MM";
        public static string FullYearNumberFormat { get; } = "yyyy";
        public static string FullMonthNameAndFullYearNumberFormat = FullMonthNameFormat + " " + FullYearNumberFormat;
    }
}
