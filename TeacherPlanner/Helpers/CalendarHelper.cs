using System;
using System.IO;
using TeacherPlanner.Login.Models;

namespace TeacherPlanner.Helpers
{
    public static class CalendarHelper
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
        private static string _timetableDirectory = "Timetables";

        static CalendarHelper()
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
        public static string GetClassCode(DateTime date, int period)
        {
            return "";
        }
        public static int GetDayNumber(string day)
        {
            switch (day.ToLower())
            {
                case "monday":
                    return 1;
                case "tuesday":
                    return 2;
                case "wednesday":
                    return 3;
                case "thursday":
                    return 4;
                case "friday":
                    return 5;
                default:
                    return 1;
            }
        }
        public static int GetWeek(DateTime date)
        {
            var filepath = Path.Combine(FileHandlingHelper.LoggedInUserDataPath, "TimetableWeeks.csv");
            var weekdata = FileHandlingHelper.ReadDataFromCSVFile(filepath);
            for (int i = 0; i < weekdata.Length; i++)
            {
                var week = weekdata[i];
                var weekstring = week[0].Split("/");
                DateTime currentWeek = new DateTime(Int32.Parse(weekstring[0]), Int32.Parse(weekstring[1]), Int32.Parse(weekstring[2]));
                DateTime nextWeek = currentWeek.AddDays(7);
                if (date >= currentWeek && date < nextWeek)
                {
                    if (week[1] == "True")
                        return 1;
                    else if (week[2] == "True")
                        return 2;
                    else if (week[3] == "True")
                        return 3;
                    else
                        return 0;
                }
            }
            return 0;
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