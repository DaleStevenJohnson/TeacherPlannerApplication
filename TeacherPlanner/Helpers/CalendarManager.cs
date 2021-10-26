using System;
using System.IO;
using static TeacherPlanner.Constants.Formats;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Constants;
using Database;
using TeacherPlanner.Planner.Models;
using System.Linq;
using TeacherPlanner.PlannerYear.Models;

namespace TeacherPlanner.Helpers
{
    public class CalendarManager : ObservableObject
    {
        // Fields
        private DateTime _today;
        private bool _datesAreNeighbours = true;
        private readonly AcademicYearModel _academicYearModel;

        public CalendarManager(AcademicYearModel academicYear)
        {
            _academicYearModel = academicYear;
            CurrentAcademicYear = academicYear.Year;
            CurrentDates = new DateTime[2];
            
            var startOfYearDate = GetStartOfYearDateLimit(CurrentAcademicYear);
            StartOfYearDateLimit = startOfYearDate;
            EndOfYearDateLimit = GetEndOfYearDateLimit(CurrentAcademicYear);

            // MUST be delcared After setting the StartOfYearDateLimit and EndOfYearDateLimit
            Today = DateTime.Today;

            CurrentDateLeft = IsAcademicYearNow(CurrentAcademicYear) ? GetAdvancedDate(Today, 0) : startOfYearDate;
            CurrentDateRight = CurrentDateLeft == EndOfYearDateLimit ? CurrentDateLeft.AddDays(0) : GetAdvancedDate(CurrentDateLeft, 1);
        }

        // Properties
        public int CurrentAcademicYear { get; set; }
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
                Tomorrow = GetAdvancedDate(_today, 1);
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
        public static int GetStartingYearOfAcademicYear(DateTime date)
        {
            var day = date.Day;
            var month = date.Month;
            var year = date.Year; 
            if ((month == 8 && day <= 15) || month > 1 && month < 8)
                return (year - 1);
            else
                return year;
        }

        public int GetWeek(DateTime date)
        {
            var timetableWeeks = DatabaseManager.GetTimetableWeeks(_academicYearModel.ID);

            if (!timetableWeeks.Any())
                return -1;
            
            foreach (var week in timetableWeeks)
            {
                DateTime nextWeek = week.WeekBeginning.AddDays(7);
                if (date >= week.WeekBeginning && date < nextWeek)
                {
                    return week.Week;
                }
            }
            // No week assigned
            return 0;
        }

        public static bool IsAcademicYearNow(int year)
        {
            var thisAcademicYearString = GetStartingYearOfAcademicYear(DateTime.Today);
            return year == thisAcademicYearString;
        }

        public static DateTime GetStartOfYearDateLimit(int AcademicYearString)
        {
            var yearInt = AcademicYearString;
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

        public static DateTime GetEndOfYearDateLimit(int AcademicYearString)
        {
            var yearInt = AcademicYearString + 1;
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
                
                if (CurrentDateLeft == StartOfYearDateLimit)
                {
                    HandleStartOfYearDayChange(days);
                }
                else
                {
                    CurrentDateLeft = GetAdvancedDate(CurrentDateLeft, days);
                    CurrentDateRight = GetAdvancedDate(CurrentDateLeft, 1);
                }
            }
            else
            {
                var side = CalculateSideToAdvance(advancePageState);
                if (side == "left")
                {
                    CurrentDateLeft = GetAdvancedDate(CurrentDateLeft, days);
                    if (CurrentDateLeft >= CurrentDateRight)
                        CurrentDateRight = GetAdvancedDate(CurrentDateLeft, 1);
                }
                else
                {
                    CurrentDateRight = GetAdvancedDate(CurrentDateRight, days);
                    if (CurrentDateLeft >= CurrentDateRight)
                        CurrentDateLeft = GetAdvancedDate(CurrentDateRight, -1);
                }
            }
        }

        public DateTime GetAdvancedDate(DateTime date, int days)
        {
            DateTime newDate;
            switch (days)
            {
                case 999:
                    newDate = date.AddMonths(1);
                    break;
                case -999:
                    newDate = date.AddMonths(-1);
                    break;
                default:
                    newDate = date.AddDays(days);
                    break;
            }

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

        // Private Methods

        private void HandleStartOfYearDayChange(int days)
        {
            // If the right hand page is at the limit and the user is trying
            // to go backwards, do nothing
            if (CurrentDateRight == StartOfYearDateLimit && days < 0)
                return;
            else if (CurrentDateRight != StartOfYearDateLimit)
                CurrentDateLeft = GetAdvancedDate(CurrentDateLeft, days);

            CurrentDateRight = GetAdvancedDate(CurrentDateRight, days);
        }

        private string CalculateSideToAdvance(AdvancePageState advancePageState)
        {
            switch (advancePageState)
            {
                case AdvancePageState.LeftForward1:
                case AdvancePageState.LeftForward7:
                case AdvancePageState.LeftForwardMonth:
                case AdvancePageState.LeftBackward1:
                case AdvancePageState.LeftBackward7:
                case AdvancePageState.LeftBackwardMonth:
                    return "left";

                case AdvancePageState.RightForward1:
                case AdvancePageState.RightForward7:
                case AdvancePageState.RightForwardMonth:
                case AdvancePageState.RightBackward1:
                case AdvancePageState.RightBackward7:
                case AdvancePageState.RightBackwardMonth:
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
                case AdvancePageState.LeftForwardMonth:
                case AdvancePageState.RightForwardMonth:
                    return 999;
                case AdvancePageState.LeftBackwardMonth:
                case AdvancePageState.RightBackwardMonth:
                    return -999;
                default:
                    return 0;
            }
        }
        
        
    }
}