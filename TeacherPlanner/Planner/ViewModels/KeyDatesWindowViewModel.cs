using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Database;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Planner.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class KeyDatesWindowViewModel : ObservableObject
    {
        private bool _isAddingNewKeyDate;
        private string _newKeyDateType;
        private string _newKeyDateTimeHour;
        private string _newKeyDateTimeMinute;
        private DateTime _newKeyDateDate;
        private string _newKeyDateDescription;
        private string _newKeyDateFeedback;
        private ObservableCollection<KeyDateItemViewModel> _keyDates;
        private bool _newKeyDateIsWeekend;
        private ObservableCollection<KeyDateItemViewModel> _deadlines;
        private readonly AcademicYearModel _academicYear;

        public ICommand SwapIsAddingNewDateValueCommand { get; }
        public ICommand AddNewKeyDateCommand { get; }
        public ICommand RemoveSelectedKeyDatesCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public event EventHandler KeyDatesListUpdatedEvent;
        public event EventHandler CloseWindowEvent;

        public KeyDatesWindowViewModel(ObservableCollection<KeyDateItemViewModel> keyDates, AcademicYearModel academicYear)
        {
            // Parameter Assignment
            KeyDates = keyDates;
            
            _academicYear = academicYear;

            // Property Assignment
            ColumnManager = new ColumnManager(new string[] { "Description", "Type", "Days Until", "Date", "Time" }, 2);
            KeyDateTypesList = new List<string>();
            foreach (KeyDateTypes keyDateType in Enum.GetValues(typeof(KeyDateTypes)))
            {
                KeyDateTypesList.Add(keyDateType.GetKeyDateTypeName());
            }

            IsAddingNewKeyDate = false;
            Today = DateTime.Today;
            NewKeyDateDate = Today;
            NewKeyDateTimeHour = "12";
            NewKeyDateTimeMinute = "00";
            NewKeyDateType = KeyDateTypesList[0];
            NewKeyDateIsWeekend = false;

            // Command Assignment
            SwapIsAddingNewDateValueCommand = new SimpleCommand(_ => OnSwapIsAddingNewDateValue());
            AddNewKeyDateCommand = new SimpleCommand(_ => OnAddNewKeyDate());
            RemoveSelectedKeyDatesCommand = new SimpleCommand(_ => RemoveKeyDates());
            CloseWindowCommand = new SimpleCommand(window => OnCloseWindow((Window)window));

            // Event Subscription
            ColumnManager.SortingChanged += (_, __) => SortKeyDates();
       

            GetKeyDates();
        }

        // Properties

        public List<string> KeyDateTypesList { get; }
        public IEnumerable<string> HoursList { get; } = new List<string> { "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18" };
        public IEnumerable<string> MinuteList { get; } = new List<string> { "00", "05", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55" };
        public bool IsAddingNewKeyDate
        {
            get => _isAddingNewKeyDate;
            set => RaiseAndSetIfChanged(ref _isAddingNewKeyDate, value);
        }
        public DateTime Today { get; set; }
        public ObservableCollection<KeyDateItemViewModel> KeyDates
        {
            get => _keyDates;
            set => RaiseAndSetIfChanged(ref _keyDates, value);
        }

       

        public ColumnManager ColumnManager { get; set; }

        // Adding new KeyDate Fields

        public string NewKeyDateFeedback
        {
            get => _newKeyDateFeedback;
            set => RaiseAndSetIfChanged(ref _newKeyDateFeedback, value);
        }

        public string NewKeyDateDescription
        {
            get => _newKeyDateDescription;
            set => RaiseAndSetIfChanged(ref _newKeyDateDescription, value);
        }

        public string NewKeyDateType
        {
            get => _newKeyDateType;
            set => RaiseAndSetIfChanged(ref _newKeyDateType, value);
        }

        public bool NewKeyDateIsWeekend
        {
            get => _newKeyDateIsWeekend;
            set => RaiseAndSetIfChanged(ref _newKeyDateIsWeekend, value);
        }

        public DateTime NewKeyDateDate
        {
            get => _newKeyDateDate;
            set
            {
                if (RaiseAndSetIfChanged(ref _newKeyDateDate, value))
                {
                    NewKeyDateIsWeekend = value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday;
                }
            }
        }

        public string NewKeyDateTimeHour
        {
            get => _newKeyDateTimeHour;
            set => RaiseAndSetIfChanged(ref _newKeyDateTimeHour, value);
        }

        public string NewKeyDateTimeMinute
        {
            get => _newKeyDateTimeMinute;
            set => RaiseAndSetIfChanged(ref _newKeyDateTimeMinute, value);
        }

        // Methods

        public void SortKeyDates()
        {
            var sortedDates = ColumnManager.Sort(KeyDates);
            KeyDates = new ObservableCollection<KeyDateItemViewModel>(sortedDates);
        }

        public void RemoveKeyDates()
        {
            var selectedDates = KeyDates.Where(kd => kd.IsChecked).ToList();
            
            foreach (KeyDateItemViewModel keydate in selectedDates)
            {
                if (DatabaseManager.TryRemoveKeyDate(keydate.ID))
                    KeyDates.Remove(keydate);
            }
                
            KeyDatesListUpdatedEvent.Invoke(null, EventArgs.Empty);
        }

        private void OnSwapIsAddingNewDateValue()
        {
            if (!IsAddingNewKeyDate)
            {
                NewKeyDateFeedback = string.Empty;
                NewKeyDateDescription = string.Empty;
            }
            IsAddingNewKeyDate = !IsAddingNewKeyDate;
        }

        private void OnCloseWindow(Window window)
        {
            CloseWindowEvent.Invoke(null, EventArgs.Empty);
            window.Close();
        }

        private void OnAddNewKeyDate()
        {
            if (NewKeyDateDescription == string.Empty)
            {
                NewKeyDateFeedback = "Please enter a description for this Key Date.";
            }
            else
            {
                AddNewKeyDate();
                KeyDatesListUpdatedEvent.Invoke(null, EventArgs.Empty);
                OnSwapIsAddingNewDateValue();
            }
        }

        private void GetKeyDates()
        {
            var dbModels = DatabaseManager.GetKeyDates(_academicYear.ID);
            if (dbModels.Any())
            {
                foreach (var model in dbModels)
                {
                    KeyDates.Add(new KeyDateItemViewModel(model.ID, model.Description, model.Type, model.DateTime));
                }
            }
        }

        private void AddNewKeyDate()
        {
            var hours = Int32.Parse(NewKeyDateTimeHour);
            var minutes = Int32.Parse(NewKeyDateTimeMinute);
            var day = NewKeyDateDate.Day;
            var month = NewKeyDateDate.Month;
            var year = NewKeyDateDate.Year;
            var date = new DateTime(year, month, day, hours, minutes, 0);

            var dbModel = new KeyDate()
            {
                AcademicYearID = _academicYear.ID,
                Description = NewKeyDateDescription,
                Type = NewKeyDateType,
                DateTime = date
            };

            if (DatabaseManager.TryAddKeyDate(dbModel, out var id))
                KeyDates.Add(new KeyDateItemViewModel(id, NewKeyDateDescription, NewKeyDateType, date));
        }
    }
}
