using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.PlannerYear.Models;
using TeacherPlanner.PlannerYear.ViewModels;
using TeacherPlanner.Timetable.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class PlannerViewModel : ObservableObject
    {
        private DayViewModel _leftDay;
        private DayViewModel _rightDay;

        private bool _singlePageScrolling = false;
        private bool _isAtEndOfYear = false;
        private bool _isAtStartOfYear = false;

        private readonly AcademicYearModel _academicYear;
        private CalendarManager _calendarManager;

        private ObservableCollection<KeyDateItemViewModel> _keyDates;

        public ICommand SaveCommand { get; }
        public ICommand GoToTodayCommand { get; }
        public PlannerViewModel(UserModel userModel, TimetableModel timetable, CalendarManager calendarManager, ObservableCollection<KeyDateItemViewModel> keyDates, AcademicYearModel academicYear)
        {
            // Parameter Assignment
            UserModel = userModel;
            Timetable = timetable;
            _calendarManager = calendarManager;
            KeyDates = keyDates;
            _academicYear = academicYear;

            // Property Assignment
            OverwriteClassCode = false;

            // Command Assignment
            GoToTodayCommand = new SimpleCommand(_ => OnGoToToday());
            SaveCommand = new SimpleCommand(_ => OnSave());

            // Method Calls
            CreateNewDayViewModels();
        }

		// Properties

		
        public TimetableModel Timetable { get; set; }
        public bool IsAtEndOfYear
        {
            get => _isAtEndOfYear;
            set => RaiseAndSetIfChanged(ref _isAtEndOfYear, value);
        }
        public bool IsAtStartOfYear
        {
            get => _isAtStartOfYear;
            set => RaiseAndSetIfChanged(ref _isAtStartOfYear, value);
        }
        public bool OverwriteClassCode { get; set; }
        public bool SinglePageScrolling 
        {
            get => _singlePageScrolling;
            set => RaiseAndSetIfChanged(ref _singlePageScrolling, value);
        }

        public UserModel UserModel { get; set; }
        public DayViewModel LeftDay
        {
            get => _leftDay;
            set => RaiseAndSetIfChanged(ref _leftDay, value);
        }
        public DayViewModel RightDay
        {
            get => _rightDay;
            set => RaiseAndSetIfChanged(ref _rightDay, value);
        }

        public ObservableCollection<KeyDateItemViewModel> KeyDates
        {
            get => _keyDates;
            set => RaiseAndSetIfChanged(ref _keyDates, value);
        }


        // Public Methods

        public void UpdateCurrentTimetable(TimetableModel timetable)
        {
            Timetable = timetable;
            LoadNewDays();
        }

        public void OnTurnPage(object parameter)
        {
            var advancePageState = (AdvancePageState)parameter;

            SaveCurrentlyDisplayedPageDays();
            _calendarManager.ChangeCurrentDate(advancePageState);
            LoadNewDays();
            //_debug.Text = $"{LeftDay.Period1.Row1.LeftText}";
        }

        public void OnSave()
        {
            SaveCurrentlyDisplayedPageDays();
            MessageBox.Show("Saved");
        }

        public void LoadNewDays()
        {
            LeftDay.LoadNewDayModel(_calendarManager.CurrentDateLeft);
            RightDay.LoadNewDayModel(_calendarManager.CurrentDateRight);

            UpdateYearLimits();
        }

        // Private Methods

        private void CreateNewDayViewModels()
        {
            LeftDay = new DayViewModel(UserModel, _calendarManager.CurrentDateLeft, Timetable, "left", KeyDates, _academicYear, _calendarManager);
            RightDay = new DayViewModel(UserModel, _calendarManager.CurrentDateRight, Timetable, "right", KeyDates, _academicYear, _calendarManager);

            LeftDay.TurnPageEvent += (_, __) => OnTurnPage(__);
            RightDay.TurnPageEvent += (_, __) => OnTurnPage(__);

            UpdateYearLimits();
        }

        private void UpdateYearLimits()
        {
            IsAtStartOfYear = RightDay.CalendarViewModel.Date == _calendarManager.StartOfYearDateLimit;
            IsAtEndOfYear = LeftDay.CalendarViewModel.Date == _calendarManager.EndOfYearDateLimit;
        }

        private void SaveCurrentlyDisplayedPageDays()
        {
            LeftDay.SaveDayToDatabase();
            RightDay.SaveDayToDatabase();
        }

        private void OnGoToToday()
        {
            _calendarManager.CurrentDateLeft = _calendarManager.GetAdvancedDate(_calendarManager.Today, 0);
            //if (CalendarManager.DatesAreNeighbours)
            _calendarManager.CurrentDateRight = _calendarManager.GetAdvancedDate(_calendarManager.CurrentDateLeft, 1);
            LoadNewDays();
        }
    }
}
