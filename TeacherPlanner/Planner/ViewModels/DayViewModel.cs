using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Database;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.PlannerYear.Models;
using TeacherPlanner.PlannerYear.ViewModels;
using TeacherPlanner.Timetable.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class DayViewModel : ObservableObject
    {
        // Fields
        private DayModel _dayModel;
        private CalendarViewModel _calendarViewModel;

        private bool _isKeyDate;
        private bool _keyDatesAreShowing;
        private bool _isDeadline;

        private readonly AcademicYearModel _academicYear;
        public readonly UserModel _userModel;
        private readonly CalendarManager _calendarManager;

        private ObservableCollection<KeyDateItemViewModel> _allKeyDates;
        private ObservableCollection<KeyDateItemViewModel> _todaysKeyDates;


        // Events / Commands / Actions
        public event EventHandler<AdvancePageState> TurnPageEvent;
        public ICommand TurnPageCommand { get; }
        public ICommand ToggleKeyDatesCommand { get; }

        // Constructor
        public DayViewModel(UserModel userModel, DateTime date, TimetableModel timetable, string side, ObservableCollection<KeyDateItemViewModel> keyDates, AcademicYearModel academicYear, CalendarManager calendarManager)
        {
            _userModel = userModel;
            Timetable = timetable;
            AllKeyDates = keyDates;
            _academicYear = academicYear;
            _calendarManager = calendarManager;
            CalendarViewModel = new CalendarViewModel(date, keyDates, calendarManager);



            IsKeyDate = false;
            IsDeadline = false;
            KeyDatesAreShowing = false;

            TurnPageCommand = new SimpleCommand(numOfDays => OnTurnPage(numOfDays));
            ToggleKeyDatesCommand = new SimpleCommand(_ => OnToggleKeyDates());


            LoadNewDayModel(date);
            SetAdvancePageStates(side);
            UpdateTodaysKeyDates();
        }



        // Properties
        // Public
        public bool IsKeyDate
        {
            get => _isKeyDate;
            set => RaiseAndSetIfChanged(ref _isKeyDate, value);
        }

        public bool IsDeadline
        {
            get => _isDeadline;
            set => RaiseAndSetIfChanged(ref _isDeadline, value);
        }

        public bool KeyDatesAreShowing
        {
            get => _keyDatesAreShowing;
            set => RaiseAndSetIfChanged(ref _keyDatesAreShowing, value);
        }

        public ObservableCollection<KeyDateItemViewModel> TodaysKeyDates
        {
            get => _todaysKeyDates;
            set => RaiseAndSetIfChanged(ref _todaysKeyDates, value);
        }


        public AdvancePageState Forward1 { get; private set; }
        public AdvancePageState Forward7 { get; private set; }
        public AdvancePageState ForwardMonth { get; private set; }
        public AdvancePageState Backward1 { get; private set; }
        public AdvancePageState Backward7 { get; private set; }
        public AdvancePageState BackwardMonth { get; private set; }
        public CalendarViewModel CalendarViewModel
        {
            get => _calendarViewModel;
            set => RaiseAndSetIfChanged(ref _calendarViewModel, value);
        }
        public DayModel DayModel
        {
            get => _dayModel;
            set => RaiseAndSetIfChanged(ref _dayModel, value);
        }

        // Private Properties
        private TimetableModel Timetable { get; }


        private ObservableCollection<KeyDateItemViewModel> AllKeyDates
        {
            get => _allKeyDates;
            set => RaiseAndSetIfChanged(ref _allKeyDates, value);
        }

        // Public Methods

        public void UpdateTodaysKeyDates()
        {
            TodaysKeyDates = new ObservableCollection<KeyDateItemViewModel>(AllKeyDates.Where(kd => kd.Date.Date == CalendarViewModel.Date.Date).OrderBy(keydate => keydate.Date));
            IsKeyDate = TodaysKeyDates.Any();
            IsDeadline = TodaysKeyDates.Where(kd => kd.Type == KeyDateTypes.Deadline.GetKeyDateTypeName()).Any();
            CalendarViewModel.SetKeyDates();
        }



        public void LoadNewDayModel(DateTime date)
        {
            KeyDatesAreShowing = false;

            // Load Day from Database
            var dayDBModel = DatabaseModelManager.GetDayDBModel(date, _academicYear.ID);

            // Load Periods from Database
            var periodModels = DatabaseModelManager.GetTodaysPeriodModels(dayDBModel, Timetable, _calendarManager);

            DayModel = new DayModel(dayDBModel, periodModels);
            CalendarViewModel = new CalendarViewModel(date, _allKeyDates, _calendarManager);
            UpdateTodaysKeyDates();
        }

        internal void SaveDayToDatabase()
        {

            // Add periods to Database
            DayModel.Periods = DatabaseModelManager.TryUpdatePeriods(DayModel.Periods);

            // Add Day to Database
            DayModel.ID = DatabaseModelManager.TryUpdateDay(DayModel);
        }



        private string GetClassCodeFromTimetable(DateTime date, int period)
        {
            if (Timetable == null)
                return string.Empty;
            var day = (int)date.DayOfWeek;
            var week = _calendarManager.GetWeek(date);
            if (week == 1 || week == 2)
            {
                // Todo maybe fix me
                var timetablePeriodModel = Timetable.GetPeriod(week, day, (PeriodCodes)period);
                return timetablePeriodModel.ClassCode;
            }
            else if (week == 3)
                return "Holiday";
            return string.Empty;
        }

        

        private void OnTurnPage(object v)
        {
            TurnPageEvent.Invoke(null, (AdvancePageState)v);
        }

        private void OnToggleKeyDates()
        {
            KeyDatesAreShowing = !KeyDatesAreShowing;
        }

        private void SetAdvancePageStates(string side)
        {
            if (side == "left")
            {
                Forward1 = AdvancePageState.LeftForward1;
                Forward7 = AdvancePageState.LeftForward7;
                ForwardMonth = AdvancePageState.LeftForwardMonth;
                Backward1 = AdvancePageState.LeftBackward1;
                Backward7 = AdvancePageState.LeftBackward7;
                BackwardMonth = AdvancePageState.LeftBackwardMonth;
            }
            else
            {
                Forward1 = AdvancePageState.RightForward1;
                Forward7 = AdvancePageState.RightForward7;
                ForwardMonth = AdvancePageState.RightForwardMonth;
                Backward1 = AdvancePageState.RightBackward1;
                Backward7 = AdvancePageState.RightBackward7;
                BackwardMonth = AdvancePageState.RightBackwardMonth;
            }
        }
    }
}
