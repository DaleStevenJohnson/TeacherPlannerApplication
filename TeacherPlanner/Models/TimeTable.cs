using System;

namespace TeacherPlanner.Models
{
    public static class TimeTable
    {
        // Fields
        private static string _dateHeadingFormat = "dddd, dd MMMM";
        private static DateTime _today;
        private static string _todayString;
        private static DateTime _tomorrow;
        private static string _tomorrowString;
        private static DateTime _currentDateLeft;
        private static string _currentDateLeftString;
        private static DateTime _currentDateRight;
        private static string _currentDateRightString;

        static TimeTable()
        {
            Today = DateTime.Today;
            CurrentDateLeft = Today;
            
        }

        // Properties
        public static DateTime Today {
            get {
                return _today;            
            } 
            set { 
                _today = value;
                _todayString = _today.ToString(_dateHeadingFormat);
                _tomorrow = AdvanceDate(_today, 1);
                _tomorrowString = _tomorrow.ToString(_dateHeadingFormat);
            } 
        }

        private static bool DatesAreNeighbours = true;

        public static string TodayString { get { return _todayString; } }

        public static DateTime Tomorrow { get { return _tomorrow; } }
                
        public static string TomorrowString { get { return _tomorrowString; } }

        public static DateTime CurrentDateLeft {
            get 
            {
                return _currentDateLeft;
            }
            set 
            {
                _currentDateLeft = value;
                _currentDateLeftString = _currentDateLeft.ToString(_dateHeadingFormat);
                if (DatesAreNeighbours) CurrentDateRight = AdvanceDate(_currentDateLeft, 1);
                
            }
                }

        public static string CurrentDateLeftString { get { return _currentDateLeftString; } }

        public static DateTime CurrentDateRight
        {
            get
            {
                return _currentDateRight;
            }
            set
            {
                _currentDateRight = value;
                _currentDateRightString = _currentDateRight.ToString(_dateHeadingFormat);
                if (DatesAreNeighbours && _currentDateLeftString != AdvanceDate(_currentDateRight, -1).ToString(_dateHeadingFormat))
                    CurrentDateLeft = AdvanceDate(_currentDateRight, -1);
            }
        }

        public static string CurrentDateRightString { get { return _currentDateRightString; } }

        public static void ChangeCurrentDate(int days, string side)
        {
            if (side.ToLower() == "left") CurrentDateLeft = AdvanceDate(CurrentDateLeft, days);
            else CurrentDateRight = AdvanceDate(CurrentDateRight, days);
        }

        private static DateTime AdvanceDate(DateTime date, int days)
        {
            DateTime newDate = date.AddDays(days);
            string day = newDate.ToString("dddd");
            int extraDays = 0;
            if (day == "Saturday") {
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
