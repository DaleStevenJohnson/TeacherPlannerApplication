using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Database;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
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
            CalendarViewModel = new CalendarViewModel(date, keyDates);

            

            IsKeyDate = false;
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
            CalendarViewModel.SetKeyDates();
        }



        public void LoadNewDayModel(DateTime date)
        {

            // Load Day from Database
            var dayDBModel = DatabaseManager.GetDay(_academicYear.ID, date);
            if (dayDBModel == null)
            {
                var newDBDay = new Day()
                {
                    AcademicYearID = _academicYear.ID,
                    Date = date,
                    Notes = null,
                };

                if (DatabaseManager.TryAddDay(newDBDay, out var id))
                {
                    dayDBModel = newDBDay;
                    dayDBModel.ID = id;
                }
                else
                {
                    // Todo - implement this better
                    MessageBox.Show("Error Loading Day");
                    return;
                }
            }

            // Load Periods from Database
            var PERIODS = 9;
            var periodDBModels = DatabaseManager.GetPeriods(dayDBModel.ID);
            var periodModels = new ObservableCollection<PeriodModel>();
            
            for (var i = 0; i < PERIODS; i++)
            {
                Period periodDBModel;

                if (periodDBModels.Count != 0 && i < periodDBModels.Count && periodDBModels[i].PeriodNumber == (int)GetPeriodCode(i))
                {
                    periodDBModel = periodDBModels[i];
                }
                else
                {
                    periodDBModel = new Period()
                    {
                        DayID = dayDBModel.ID,
                        TimetableClasscode = GetTimetablePeriodID(date, i),
                        UserEnteredClasscode = null,
                        PeriodNumber = (int)GetPeriodCode(i),
                        MarginText = null,
                        MainText = null,
                        SideText = null
                    };
                }
                    
                var periodModel = new PeriodModel(periodDBModel);
                periodModels.Add(periodModel);
            }

            DayModel = new DayModel(dayDBModel, periodModels);
        }

        internal void SaveDayToDatabase()
        {
            
            // Add periods to Database
            foreach (var period in DayModel.Periods)
            {
                var dbModel = period.GetDBModel();
                if (!DatabaseManager.TryUpdatePeriod(dbModel))
                {
                    if (DatabaseManager.TryAddPeriod(dbModel, out var id))
                    {
                        period.ID = id;
                    }
                    else
                    {
                        MessageBox.Show("Error saving Period to Database");
                    }
                }
            }

            // Add Day to Database

            var day = DayModel.GetDBModel();

            if (!DatabaseManager.TryUpdateDay(day))
            {
                if (DatabaseManager.TryAddDay(day, out var id))
                {
                    DayModel.ID = id;
                }
                else
                {
                    // Todo make this better
                    MessageBox.Show("Failed to Save Day to Database");
                }
            }
        }

        private PeriodCodes GetPeriodCode(int period)
        {
            switch (period)
            {
                case 0:
                    return PeriodCodes.Registration;
                case 1:
                    return PeriodCodes.Period1;
                case 2:
                    return PeriodCodes.Period2;
                case 3:
                    return PeriodCodes.Break;
                case 4:
                    return PeriodCodes.Period3;
                case 5:
                    return PeriodCodes.Lunch;
                case 6:
                    return PeriodCodes.Period4;
                case 7:
                    return PeriodCodes.Period5;
                case 8:
                    return PeriodCodes.Twilight;
                default:
                    return PeriodCodes.Registration;

            }
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

        private int? GetTimetablePeriodID(DateTime date, int period)
        {
            if (Timetable == null)
                return null;
            
            var day = (int)date.DayOfWeek;
            var week = _calendarManager.GetWeek(date);
            
            if (week == 1 || week == 2)
            {
                // Todo maybe fix me
                var timetablePeriodModel = Timetable.GetPeriod(week, day, (PeriodCodes)period);
                return timetablePeriodModel.ID;
            }

            return null;
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
