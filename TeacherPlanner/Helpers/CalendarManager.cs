using System;
using System.IO;
using static TeacherPlanner.Constants.Formats;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Helpers
{
    public class CalendarManager : ObservableObject
    {
        // Fields
        private DateTime _today;
        private bool _datesAreNeighbours = true;

        public CalendarManager(string academicYear)
        {
            CurrentAcademicYear = academicYear.Substring(0, 4);
            CurrentDates = new DateTime[2];
            
            var startOfYearDate = GetStartOfYearDateLimit(CurrentAcademicYear);
            StartOfYearDateLimit = startOfYearDate;
            EndOfYearDateLimit = GetEndOfYearDateLimit(CurrentAcademicYear);

            // MUST be delcared After setting the StartOfYearDateLimit and EndOfYearDateLimit
            Today = DateTime.Today;

            CurrentDateLeft = IsAcademicYearNow(CurrentAcademicYear) ? Today : startOfYearDate;
            CurrentDateRight = CurrentDateLeft == EndOfYearDateLimit ? CurrentDateLeft.AddDays(0) : CurrentDateLeft.AddDays(1);
        }

        // Properties
        public string CurrentAcademicYear { get; set; }
        public DateTime[] CurrentDates { get; set; }

        public DateTime Today
        {
            get => _today;
            set
            {
                if (value <= StartOfYearDateLimit)
                    _today = StartOfYearDateLimit;
                else if (value >= EndOfYearDateLimit)
                    _today = EndOfYearDateLimit;
                else
                    _today = value;
                TodayString = _today.ToString(DateHeadingFormat);
                Tomorrow = AdvanceDate(_today, 1);
                TomorrowString = Tomorrow.ToString(DateHeadingFormat);
            }
        }

        public bool DatesAreNeighbours 
        { 
            get => _datesAreNeighbours; 
            set => RaiseAndSetIfChanged(ref _datesAreNeighbours, value); 
        }

        public string TodayString { get; private set; }

        public DateTime Tomorrow { get; private set; }

        public string TomorrowString { get; private set; }

        public DateTime CurrentDateLeft
        {
            get => CurrentDates[0];
            set
            {
                CurrentDates[0] = value;
            }
        }

        public DateTime CurrentDateRight
        {
            get => CurrentDates[1];
            set
            {
                CurrentDates[1] = value;
            }
        }

        public DateTime StartOfYearDateLimit { get; private set; }
        public DateTime EndOfYearDateLimit { get; private set; }

        // Static Methods
        public static string GetStartingYearOfAcademicYear(DateTime date)
        {
            var day = Int32.Parse(date.ToString("dd"));
            var month = Int32.Parse(date.ToString("MM"));
            var year = Int32.Parse(date.ToString("yyyy"));
            if ((month == 8 && day <= 15) || month > 1 && month < 8)
                return (year - 1).ToString();
            else
                return year.ToString();
        }

        public static int GetWeek(DateTime date)
        {
            var filepath = Path.Combine(FileHandlingHelper.LoggedInUserConfigPath, FilesAndDirectories.TimetableWeeksFileName);
            var weekdata = FileHandlingHelper.ReadDataFromCSVFile(filepath);
            for (var i = 0; i < weekdata.Length; i++)
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
        public static bool IsAcademicYearNow(string year)
        {
            var thisAcademicYearString = GetStartingYearOfAcademicYear(DateTime.Today);
            return year == thisAcademicYearString;
        }

        public static DateTime GetStartOfYearDateLimit(string AcademicYearString)
        {
            var yearInt = Int32.Parse(AcademicYearString);
            var month = 8;
            var day = 21;
            var date = new DateTime(yearInt, month, day);
            while (date.ToString("dddd").ToLower() != "monday")
            {
                day += 1;
                date = date.AddDays(1);
            }
            return new DateTime(yearInt, month, day);
        }

        public static DateTime GetEndOfYearDateLimit(string AcademicYearString)
        {
            var yearInt = Int32.Parse(AcademicYearString) + 1;
            var month = 8;
            var day = 7;
            var date = new DateTime(yearInt, month, day);
            while (date.ToString("dddd").ToLower() != "friday")
            {
                day += 1;
                date = date.AddDays(1);
            }
            return new DateTime(yearInt, month, day);
        }

        // Non-Static Methods
        public void ChangeCurrentDate(AdvancePageState advancePageState)
        {
            var days = CalculateDaysToAdvance(advancePageState);
            if (DatesAreNeighbours)
            {
                CurrentDateLeft = AdvanceDate(CurrentDateLeft, days);
                CurrentDateRight = AdvanceDate(CurrentDateRight, days);
            }
            else
            {
                var side = CalculateSideToAdvance(advancePageState);
                if (side == "left")
                    CurrentDateLeft = AdvanceDate(CurrentDateLeft, days);
                else
                    CurrentDateRight = AdvanceDate(CurrentDateRight, days);
            }
        }
        private string CalculateSideToAdvance(AdvancePageState advancePageState)
        {
            switch (advancePageState)
            {
                case AdvancePageState.LeftForward1:
                case AdvancePageState.LeftForward7:
                case AdvancePageState.LeftBackward1:
                case AdvancePageState.LeftBackward7:
                    return "left";

                case AdvancePageState.RightForward1:
                case AdvancePageState.RightForward7:
                case AdvancePageState.RightBackward1:
                case AdvancePageState.RightBackward7:
                    return "right";
                default:
                    return string.Empty;
            }
        }
        private int CalculateDaysToAdvance(AdvancePageState advancePageState)
        {
            switch (advancePageState)
            {
                case AdvancePageState.LeftForward1:
                case AdvancePageState.RightForward1:
                    return 1;
                case AdvancePageState.LeftForward7:
                case AdvancePageState.RightForward7:
                    return 7;
                case AdvancePageState.LeftBackward1:
                case AdvancePageState.RightBackward1:
                    return -1;
                case AdvancePageState.LeftBackward7:
                case AdvancePageState.RightBackward7:
                    return -7;
                default:
                    return 0;
            }
        }
        
        private DateTime AdvanceDate(DateTime date, int days)
        {
            //DateTime oldDate = date.AddDays(0);
            DateTime newDate = date.AddDays(days);
            var extraDays = 0;
            if (newDate.DayOfWeek == DayOfWeek.Saturday)
            {
                extraDays = days < 0 ? -1 : 2;
            }
            else if (newDate.DayOfWeek == DayOfWeek.Sunday)
            {
                extraDays = days < 0 ? -2 : 1;
            }
            newDate = newDate.AddDays(extraDays);
            
            if (newDate < StartOfYearDateLimit)
                return StartOfYearDateLimit;
            else if (newDate > EndOfYearDateLimit)
                return EndOfYearDateLimit;

            return newDate;
        }
    }
}