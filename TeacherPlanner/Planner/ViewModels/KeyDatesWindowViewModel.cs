using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Helpers;

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

        public ICommand SwapIsAddingNewDateValueCommand { get; }
        public ICommand AddNewKeyDateCommand { get; }
        public ICommand RemoveSelectedKeyDatesCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public event EventHandler KeyDatesListUpdatedEvent;
        public event EventHandler CloseWindowEvent;

        public KeyDatesWindowViewModel(ObservableCollection<KeyDateItemViewModel> keyDates)
        {
            // Parameter Assignment
            KeyDates = keyDates;

            // Property Assignment
            ColumnManager = new ColumnManager(new string[] { "Description", "Type", "Date", "Time" }, 2);
            KeyDateTypes = new List<string> { "Parent's Evening", "Report", "Event", "CPD", "Meeting", "Other" };
            IsAddingNewKeyDate = false;
            Today = DateTime.Today;
            NewKeyDateDate = Today;
            NewKeyDateTimeHour = "12";
            NewKeyDateTimeMinute = "00";
            NewKeyDateType = KeyDateTypes[0];
            NewKeyDateIsWeekend = false;

            // Test Data
            AddNewKeyDate("Year 12", "Event", DateTime.Now);
            AddNewKeyDate("Reading", "CPD", DateTime.Now.AddDays(5));
            AddNewKeyDate("Year 9", "Parent's Evening", DateTime.Now.AddDays(10));

            // Command Assignment
            SwapIsAddingNewDateValueCommand = new SimpleCommand(_ => OnSwapIsAddingNewDateValue());
            AddNewKeyDateCommand = new SimpleCommand(_ => OnAddNewKeyDate());
            RemoveSelectedKeyDatesCommand = new SimpleCommand(_ => RemoveKeyDates());
            CloseWindowCommand = new SimpleCommand(window => OnCloseWindow((Window)window));

            // Event Subscription
            ColumnManager.SortingChanged += (_, __) => SortKeyDates();

            
            

        }
        public List<string> KeyDateTypes { get; }
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

        public void SortKeyDates()
        {
            var sortedDates = ColumnManager.Sort(KeyDates);
            KeyDates = new ObservableCollection<KeyDateItemViewModel>(sortedDates);
        }

        public void RemoveKeyDates()
        {
            var selectedDates = KeyDates.Where(kd => kd.IsChecked).ToList();
            
            foreach (KeyDateItemViewModel keydate in selectedDates)
                KeyDates.Remove(keydate);

            KeyDatesListUpdatedEvent.Invoke(null, EventArgs.Empty);
            // Todo - Remove Keydate from the Database
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
           // KeyDatesListUpdatedEvent = null;
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

        private void AddNewKeyDate(string description = null, string type = null, DateTime date = default)
        {
            KeyDateItemViewModel keydate;
            if (description == null)
                description = NewKeyDateDescription;

            if (type == null)
                type = NewKeyDateType;

            if (date == DateTime.MinValue)
            {
                var hours = Int32.Parse(NewKeyDateTimeHour);
                var minutes = Int32.Parse(NewKeyDateTimeMinute);
                var day = Int32.Parse(NewKeyDateDate.ToString("dd"));
                var month = Int32.Parse(NewKeyDateDate.ToString("MM"));
                var year = Int32.Parse(NewKeyDateDate.ToString("yyyy"));
                date = new DateTime(year, month, day, hours, minutes, 0);
            }
            
            keydate = new KeyDateItemViewModel(description, type, date);

            KeyDates.Add(keydate);
        }
    }
}
