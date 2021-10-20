using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Database;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.PlannerYear.Models;
using TeacherPlanner.Timetable.Models;
using TeacherPlanner.Timetable.ViewModels;

namespace TeacherPlanner.LessonSequence.ViewModels
{
    public class LessonSequenceViewModel : ObservableObject
    {
        private readonly AcademicYearModel _academicYear;
        private readonly TimetableModel _timetable;
        private readonly CalendarManager _calendarManager;
        private string _selectedClassCode;
        private DateTime _selectedDate;
        private ObservableCollection<DayModel> _lessonSequence;
        private TimetableViewModel _selectedClassCodeTimetable;

        public ICommand ChangeDateCommand { get; }
        public ICommand ChangeDateToTodayCommand { get; }
        public LessonSequenceViewModel(AcademicYearModel academicYear, TimetableModel timetable, CalendarManager calendarManager, UserModel userModel)
        {
            _academicYear = academicYear;
            _timetable = timetable;
            _calendarManager = calendarManager;


            ClassCodes = new ObservableCollection<string>(_timetable.GetUniqueClassCodes());
            // Need to use backing field here to avoid hitting the setter and
            // getting into an annoying chicken and egg situation
            _selectedClassCode = ClassCodes[0];
            _selectedDate = DateTime.Today;
            LessonSequence = GetLessonSequence(SelectedDate, SelectedClassCode);
            SelectedClassCodeTimetable = new TimetableViewModel(userModel, calendarManager, academicYear);

            ChangeDateCommand = new SimpleCommand(daysToAdd => IncrementDate(Int32.Parse((string)daysToAdd)));
            ChangeDateToTodayCommand = new SimpleCommand(_ => OnChangeDateToToday());
        }

        public ObservableCollection<DayModel> LessonSequence 
        {
            get => _lessonSequence;
            set => RaiseAndSetIfChanged(ref _lessonSequence, value);
        }

        public ObservableCollection<string> ClassCodes { get; }

        public TimetableViewModel SelectedClassCodeTimetable 
        {
            get => _selectedClassCodeTimetable;
            set => RaiseAndSetIfChanged(ref _selectedClassCodeTimetable, value);
        }

        public DateTime SelectedDate 
        {
            get => _selectedDate;
            set
            {
                if (RaiseAndSetIfChanged(ref _selectedDate, value))
                {
                    UpdateLessonSequence();
                }
            }

        }

        public string SelectedClassCode 
        {
            get => _selectedClassCode;
            set
            {
                if (RaiseAndSetIfChanged(ref _selectedClassCode, value))
                {
                    UpdateLessonSequence();
                    UpdateSelectedClassCodeTimetable();
                }
            }
        }
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

        private void UpdateLessonSequence()
        {
            LessonSequence = GetLessonSequence(SelectedDate, SelectedClassCode);
        }

        private void IncrementDate(int increment)
        {
            SelectedDate = SelectedDate.AddDays(increment);
        }

        private void OnChangeDateToToday()
        {
            SelectedDate = DateTime.Today;
        }

        private void UpdateSelectedClassCodeTimetable()
        {
            var dbModels = DatabaseManager.GetTimetablePeriods(_academicYear.ID);
            SelectedClassCodeTimetable.CurrentTimetable.Update(dbModels);
            SelectedClassCodeTimetable.CurrentTimetable.Filter(new List<string>() { SelectedClassCode });
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
