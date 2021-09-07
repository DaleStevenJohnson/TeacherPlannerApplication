using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.Timetable.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class PlannerViewModel : ObservableObject
    {
        private const int DAYLIMIT = 14;
        private DayViewModel _leftDay;
        private DayViewModel _rightDay;
        private int _loadedDayModelsIndex = 0;
        private bool _singlePageScrolling = false;
        private bool _isAtEndOfYear = false;
        private bool _isAtStartOfYear = false;
        private ObservableCollection<KeyDateItemViewModel> _keyDates;

        public ICommand SaveCommand { get; }
        public ICommand GoToTodayCommand { get; }
        public PlannerViewModel(UserModel userModel, TimetableModel timetable, CalendarManager calendarManager, ObservableCollection<KeyDateItemViewModel> keyDates)
        {
            // Parameter Assignment
            UserModel = userModel;
            Timetable = timetable;
            CalendarManager = calendarManager;
            KeyDates = keyDates;

            // Property Assignment
            //LoadedDayModels = new DayModel[DAYLIMIT];

            
            OverwriteClassCode = false;

            LoadNewDays();

            // Command Assignment
            //TurnPageForwardCommand = new SimpleCommand(numOfDays => OnTurnPageForward(Convert.ToInt32(numOfDays)));
            //TurnPageBackwardCommand = new SimpleCommand(numOfDays => OnTurnPageBackward(Convert.ToInt32(numOfDays)));
            GoToTodayCommand = new SimpleCommand(_ => OnGoToToday());
            SaveCommand = new SimpleCommand(_ => OnSave());
        }

		public void UpdateCurrentTimetable(TimetableModel timetable)
		{
            Timetable = timetable;
            LoadNewDays();
		}

		public CalendarManager CalendarManager { get; }
        public TimetableModel Timetable { get; set; }
        public DayModel[] LoadedDayModels { get; set; }
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


        // TODO - Fix page turning Bug - most likely a fault on how
        // the CalendarManager is advancing day
        public void OnTurnPage(object parameter)
        {
            var advancePageState = (AdvancePageState)parameter;

            SaveCurrentlyDisplayedPageDays();
            CalendarManager.ChangeCurrentDate(advancePageState);
            LoadNewDays();
            //_debug.Text = $"{LeftDay.Period1.Row1.LeftText}";
        }
        public void OnSave()
        {
            SaveCurrentlyDisplayedPageDays();
            MessageBox.Show("Saved");
        }

        private void SaveCurrentlyDisplayedPageDays()
        {
            LeftDay.SaveDayToFile();
            RightDay.SaveDayToFile();
        }

        private void OnGoToToday()
        {
            CalendarManager.CurrentDateLeft = CalendarManager.GetAdvancedDate(CalendarManager.Today, 0);
            //if (CalendarManager.DatesAreNeighbours)
            CalendarManager.CurrentDateRight = CalendarManager.GetAdvancedDate(CalendarManager.CurrentDateLeft, 1);
            LoadNewDays();
        }

        public void LoadNewDays()
        {
            LeftDay = new DayViewModel(UserModel, CalendarManager.CurrentDateLeft, Timetable, "left", KeyDates);
            RightDay = new DayViewModel(UserModel, CalendarManager.CurrentDateRight, Timetable, "right", KeyDates);
            LeftDay.TurnPageEvent += (_, __) => OnTurnPage(__);
            RightDay.TurnPageEvent += (_, __) => OnTurnPage(__);
            IsAtStartOfYear = RightDay.CalendarViewModel.Date == CalendarManager.StartOfYearDateLimit;
            IsAtEndOfYear = LeftDay.CalendarViewModel.Date == CalendarManager.EndOfYearDateLimit;
        }

  
        private void AddDayToLoadedDayModelList(DayModel day)
        {
            LoadedDayModels[_loadedDayModelsIndex] = day;
            _loadedDayModelsIndex++;
            if (_loadedDayModelsIndex >= DAYLIMIT)
            {
                _loadedDayModelsIndex = 0;
            }
        }
        private void MoveLeft()
        {
            for (int i = 1; i < LoadedDayModels.Length; i++)
            {
                LoadedDayModels[i - 1] = LoadedDayModels[i];
            }
        }
        private void MoveRight()
        {
            for (int i = LoadedDayModels.Length - 2; i > -1; i--)
            {
                LoadedDayModels[i + 1] = LoadedDayModels[i];
            }
        }



    }


}
