using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.PlannerYear.Models;
using TeacherPlanner.Timetable.Models;

namespace TeacherPlanner.LessonSequence.ViewModels
{
    public class LessonSequenceViewModel
    {
        private readonly AcademicYearModel _academicYear;
        private readonly TimetableModel _timetable;
        private readonly CalendarManager _calendarManager;
        public LessonSequenceViewModel(AcademicYearModel academicYear, TimetableModel timetable, CalendarManager calendarManager)
        {
            _academicYear = academicYear;
            _timetable = timetable;
            _calendarManager = calendarManager;

            LessonSequence = GetLessonSequence(new DateTime(2021, 8, 21), "12B/Cp1");
        }

        public ObservableCollection<DayModel> LessonSequence { get; set; }

        public ObservableCollection<DayModel> GetLessonSequence(DateTime date, string classcode)
        {
            // Identify all the lessons needed
            // 1 in the past, 5 in the future
            var dates = GetLessonSequenceDates(date, classcode).Distinct().ToList();

            // Read any periods from database already stored there
            var days = DatabaseModelManager.GetLessonSequenceDayModels(dates, classcode, _academicYear.ID, _timetable, _calendarManager);



            // Some periods will not already be stored, so they will need creating.
            return days;
        }

        private List<DateTime> GetLessonSequenceDates(DateTime date, string classcode)
        {
            // Note: Some days might have more than one lesson of the same classcode
            // In this instance, we want to record both lessons as individual spots in the dates list.

            // Find the date of the most recent lesson(s) that has already happened
            var dates = FindDateOfNextLessons(date, classcode, out _, -1);

            // If there were no lessons previously, find the date of the next upcoming future lesson(s)
            if (!dates.Any())
                dates = FindDateOfNextLessons(date.AddDays(-1), classcode, out _);

            // Keep searching for lessons until 6 have been found in total.
            while (dates.Count < 7)
            {
                var nextLessons = FindDateOfNextLessons(dates.Last(), classcode, out var atEndOfYearLimit);
                
                foreach(var d in nextLessons)
                {
                    dates.Add(d);
                }

                if (atEndOfYearLimit)
                    break;
            }
            return dates;
        }

        private List<DateTime> FindDateOfNextLessons(DateTime date, string classcode, out bool atEndOfYearLimit, int advanceAmount = 1)
        {
            atEndOfYearLimit = false;
            var dates = new List<DateTime>();
            for (int i = 0; i < 10; i++)
            {
                date = AdvanceDate(date, advanceAmount);
                if (date < _calendarManager.StartOfYearDateLimit)
                {
                    if (date > _calendarManager.EndOfYearDateLimit)
                        atEndOfYearLimit = true;
                    return dates;
                }

                var week = _calendarManager.GetWeek(date);
                var day = (int)date.DayOfWeek;
                for (int j = 1; j < 6; j++)
                {
                    var period = _timetable.GetPeriod(week, day, (PeriodCodes)j);
                    if (period.ClassCode == classcode)
                    {
                        dates.Add(date);
                    }
                }
                if (dates.Count > 0)
                    return dates;
            }
            return dates;
        }

        private DateTime AdvanceDate(DateTime date, int amount)
        {
            date = date.AddDays(amount);
            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(amount);
            }
            return date;
        }


    }
}
