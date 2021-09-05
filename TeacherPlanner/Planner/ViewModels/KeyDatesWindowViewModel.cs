using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

        public ICommand SwapIsAddingNewDateValueCommand { get; }
        public ICommand AddNewKeyDateCommand { get; }

        public KeyDatesWindowViewModel()
        {
            KeyDates = new ObservableCollection<KeyDateItemViewModel>();
            KeyDates.Add(new KeyDateItemViewModel("Year 13", "Parent's Evening", new DateTime(2021, 9, 9)));
            KeyDates.Add(new KeyDateItemViewModel("Safegaurding", "CPD", new DateTime(2021, 10, 21)));
            KeyDates.Add(new KeyDateItemViewModel("Year 10", "Parent's Evening", new DateTime(2021, 9, 13)));
            ColumnManager = new ColumnManager(new string[]{ "Description", "Type", "Date", "Time" }, 2);

            SwapIsAddingNewDateValueCommand = new SimpleCommand(_ => OnSwapIsAddingNewDateValue());
            AddNewKeyDateCommand = new SimpleCommand(_ => OnAddNewKeyDate());
            ColumnManager.SortingChanged += (_,__) => SortKeyDates();
            
            KeyDateTypes = new List<string> { "Parent's Evening", "Report", "Event", "CPD", "Meeting", "Other" };
            IsAddingNewKeyDate = false;
            Today = DateTime.Today;
            NewKeyDateDate = Today;
            NewKeyDateTimeHour = "12";
            NewKeyDateTimeMinute = "00";
            NewKeyDateType = KeyDateTypes[0];
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
        public ObservableCollection<KeyDateItemViewModel> KeyDates { get; private set; }

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

        public DateTime NewKeyDateDate
        {
            get => _newKeyDateDate;
            set => RaiseAndSetIfChanged(ref _newKeyDateDate, value);
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

        private void OnSwapIsAddingNewDateValue()
        {
            if (!IsAddingNewKeyDate)
            {
                NewKeyDateFeedback = string.Empty;
                NewKeyDateDescription = string.Empty;
            }
            IsAddingNewKeyDate = !IsAddingNewKeyDate;
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
                OnSwapIsAddingNewDateValue();
            }
        }

        private void AddNewKeyDate()
        {
            var hours = Int32.Parse(NewKeyDateTimeHour);
            var minutes = Int32.Parse(NewKeyDateTimeMinute);
            var day = Int32.Parse(NewKeyDateDate.ToString("dd"));
            var month = Int32.Parse(NewKeyDateDate.ToString("MM"));
            var year = Int32.Parse(NewKeyDateDate.ToString("yyyy"));
            DateTime keydatetime = new DateTime(year, month, day, hours, minutes, 0);
            var keydate = new KeyDateItemViewModel(NewKeyDateDescription,NewKeyDateType,keydatetime);
            KeyDates.Add(keydate);
        }
        public void SortKeyDates()
        {
            var sortedDates = ColumnManager.Sort(KeyDates);
            KeyDates = new ObservableCollection<KeyDateItemViewModel>(sortedDates);
        }
    }
}
