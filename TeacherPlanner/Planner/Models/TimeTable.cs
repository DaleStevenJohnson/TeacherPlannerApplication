using System;
using System.IO;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;

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
        private static string _timetableDirectory = "Timetables";

        static TimeTable()
        {
            CurrentDates = new DateTime[2];
            Today = DateTime.Today;
            CurrentDateLeft = Today;

            DefineWeeks();
        }

        // Properties
        public static TimetableModel CurrentTimetable { get; set; }

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
        public static bool WeeksAreDefined { get; set; }


        // Methods
        public static void DefineWeeks()
        {
            WeeksAreDefined = false;
        }

        /// <summary>
        /// Takes a SIMS created timetable file and saves it locally for future use by the application
        /// </summary>
        /// <param name="timetableFilePath"></param>
        public static bool ImportTimetable(string timetableFilePath, string name, UserModel userModel)
        {
            string[][] rawTimetableFileData = FileHandlingHelper.ReadDataFromCSVFile(timetableFilePath);
            if (TryParseTimetableFileData(rawTimetableFileData))
            {
                string[][] convertedTimetableData = ConvertTimetableData(rawTimetableFileData);
                string path = Path.Combine(FileHandlingHelper.UserDataPath, userModel.Username, _timetableDirectory);
                FileHandlingHelper.TryWriteDataToCSVFile(path, name + ".csv", convertedTimetableData, "o", true, userModel.Key);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Takes a SIMS created timetable, strips out neccessary data and formats it in to the application's standard.
        /// </summary>
        /// <param name="rawTimetableData"></param>
        /// <returns></returns>
        private static string[][] ConvertTimetableData(string[][] rawTimetableData)
        {
            // This is unfinished! I have no idea what format the rawTimetableData will be in yet
            string[][] convertedTimetableData = new string[rawTimetableData.Length][];
            for (int row = 0; row < rawTimetableData.Length; row++)
            {
                string[] line = rawTimetableData[row];
                if (line.Length != 0)
                {
                    convertedTimetableData[row] = line;
                }
            }

            return convertedTimetableData;
        }

        /// <summary>
        /// Checks that provided SIMS timetable data file is in the correct format
        /// </summary>
        /// <param name="timetableFileData"></param>
        /// <returns></returns>
        private static bool TryParseTimetableFileData(string[][] timetableFileData)
        {
            return true;
        }
        public static void TryLoadTimetable(string timetableName)
        {
            var filepath = Path.Combine(FileHandlingHelper.UserDataPath, _timetableDirectory, timetableName + ".csv");
            var timetableData = FileHandlingHelper.ReadDataFromCSVFile(filepath);
            CurrentTimetable = new TimetableModel(timetableData, timetableName);
        }
        public static void ChangeCurrentDate(int days)
        {
            if (days < 0) CurrentDateLeft = AdvanceDate(CurrentDateLeft, days);
            else CurrentDateRight = AdvanceDate(CurrentDateRight, days);
        }
        public static string GetClassCode(DateTime date, int period)
        {
            return "";
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