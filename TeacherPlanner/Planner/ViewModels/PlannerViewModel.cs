using System;
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

        public ICommand GoToTodayCommand { get; }
        public PlannerViewModel(UserModel userModel, TimetableModel timetable, CalendarManager calendarManager)
        {
            // Parameter Assignment
            UserModel = userModel;
            Timetable = timetable;
            CalendarManager = calendarManager;

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

        public ICommand SaveCommand { get; }
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
            LeftDay = new DayViewModel(UserModel, CalendarManager.CurrentDateLeft, Timetable, "left");
            RightDay = new DayViewModel(UserModel, CalendarManager.CurrentDateRight, Timetable, "right");
            LeftDay.TurnPageEvent += (_, __) => OnTurnPage(__);
            RightDay.TurnPageEvent += (_, __) => OnTurnPage(__);
            IsAtStartOfYear = RightDay.CalendarModel.Date == CalendarManager.StartOfYearDateLimit;
            IsAtEndOfYear = LeftDay.CalendarModel.Date == CalendarManager.EndOfYearDateLimit;
        }

        private void LoadNewDay(DateTime date, DayViewModel dayViewModel)
        {
            int index = IndexOfLoadedDay(date);
            if (index != -1)
                dayViewModel.DayModel = LoadedDayModels[index];
            else
            {
                dayViewModel.DayModel = dayViewModel.LoadAndPopulateNewDay(date, OverwriteClassCode);
                AddDayToLoadedDayModelList(dayViewModel.DayModel);
            }
        }
        private bool TryRemoveLoadedDay(DateTime date)
        {
            for (int i = 0; i < LoadedDayModels.Length; i++)
            {
                if (LoadedDayModels[i] != null && LoadedDayModels[i].Date == date)
                    LoadedDayModels[i] = null;
                    return true;
            }
            return false;
        }
        private int IndexOfLoadedDay(DateTime date)
        {
            for (int i = 0; i < LoadedDayModels.Length; i++)
            {
                if (LoadedDayModels[i] != null && LoadedDayModels[i].Date == date)
                    return i;
            }
            return -1;
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
