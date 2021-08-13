using System;

namespace TeacherPlanner.Planner.Models
{
    public static class TimeTable
    {
        // Fields
        public static string DateHeadingFormat = "dddd, dd MMMM";
        private static DateTime _today;
        private static string _todayString;
        private static DateTime _tomorrow;
        private static string _tomorrowString;
        private static DateTime _currentDateLeft;
        private static string _currentDateLeftString;
        private static DateTime _currentDateRight;
        private static string _currentDateRightString;
        public static DateTime[] CurrentDates;

        static TimeTable()
        {
            CurrentDates = new DateTime[2];
            Today = DateTime.Today;
            CurrentDateLeft = Today;

        }

        // Properties
        public static DateTime Today
        {
            get
            {
                return _today;
            }
            set
            {
                _today = value;
                _todayString = _today.ToString(DateHeadingFormat);
                _tomorrow = AdvanceDate(_today, 1);
                _tomorrowString = _tomorrow.ToString(DateHeadingFormat);
            }
        }

        private static bool DatesAreNeighbours = true;

        public static string TodayString { get { return _todayString; } }

        public static DateTime Tomorrow { get { return _tomorrow; } }

        public static string TomorrowString { get { return _tomorrowString; } }

        public static DateTime CurrentDateLeft
        {
            get
            {
                return CurrentDates[0];
            }
            set
            {
                CurrentDates[0] = value;
                _currentDateLeftString = CurrentDates[0].ToString(DateHeadingFormat);
                if (DatesAreNeighbours) CurrentDateRight = AdvanceDate(CurrentDates[0], 1);

            }
        }

        public static string CurrentDateLeftString { get { return _currentDateLeftString; } }

        public static DateTime CurrentDateRight
        {
            get
            {
                return CurrentDates[1];
            }
            set
            {
                CurrentDates[1] = value;
                _currentDateRightString = CurrentDates[1].ToString(DateHeadingFormat);
                if (DatesAreNeighbours && _currentDateLeftString != AdvanceDate(CurrentDates[1], -1).ToString(DateHeadingFormat))
                    CurrentDateLeft = AdvanceDate(CurrentDates[1], -1);
            }
        }

        public static string CurrentDateRightString { get { return _currentDateRightString; } }

        public static void ChangeCurrentDate(int days)
        {
            if (days < 0) CurrentDateLeft = AdvanceDate(CurrentDateLeft, days);
            else CurrentDateRight = AdvanceDate(CurrentDateRight, days);
        }

        private static DateTime AdvanceDate(DateTime date, int days)
        {
            DateTime newDate = date.AddDays(days);
            string day = newDate.ToString("dddd");
            int extraDays = 0;
            if (day == "Saturday")
            {
                extraDays = days < 0 ? -1 : 2;
            }
            else if (day == "Sunday")
            {
                extraDays = days < 0 ? -2 : 1;
            }
            return newDate.AddDays(extraDays);
        }
    }
}
