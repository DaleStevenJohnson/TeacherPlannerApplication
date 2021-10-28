using System;
using System.Collections.Generic;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.AcademicYearConfiguration.Models
{
    public class PeriodTimingsModel : ObservableObject
    {
        private string _startTimeHour;
        private string _startTimeMinute;
        private string _endTimeHour;
        private string _endTimeMinute;

        public event EventHandler TimeUpdatedEvent;
        public PeriodTimingsModel(string periodType, PeriodCodes periodCode)
        {
            // Parameter Assignment
            Type = periodType;
            PeriodCode = periodCode;

            // Property Assignment
            _startTimeHour = ComboBoxLists.HoursList[0];
            _startTimeMinute = ComboBoxLists.MinuteList[0];
            _endTimeHour = ComboBoxLists.HoursList[0];
            _endTimeMinute = ComboBoxLists.MinuteList[0];

        }
        public string DisplayName
        {
            get => $"{Type} {((int)PeriodCode) % 100}";
        }
        public PeriodCodes PeriodCode { get; set; }
        public string Type { get; set; }
        public string StartTimeHour
        {
            get => _startTimeHour;
            set
            {
                _startTimeHour = value;
                TimingsUpdated();
            }
        }
        public string StartTimeMinute
        {
            get => _startTimeMinute;
            set
            {
                _startTimeMinute = value;
                TimingsUpdated();
            }
        }
        public string EndTimeHour
        {
            get => _endTimeHour;
            set
            {
                _endTimeHour = value;
                TimingsUpdated();
            }
            }
        public string EndTimeMinute
        {
            get => _endTimeMinute;
            set
            {
                _endTimeMinute = value;
                TimingsUpdated();
            }
        }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        private void TimingsUpdated()
        {
            StartTime = new DateTime(2000, 1, 1, int.Parse(StartTimeHour), int.Parse(StartTimeMinute), 1);
            EndTime = new DateTime(2000, 1, 1, int.Parse(EndTimeHour), int.Parse(EndTimeMinute), 0);
            TimeUpdatedEvent.Invoke(null, EventArgs.Empty);
        }
    }


    public class PeriodTimingsModelComparer : IComparer<PeriodTimingsModel>
    {
        public int Compare(PeriodTimingsModel x, PeriodTimingsModel y)
        {
            return x.StartTime.CompareTo(y.StartTime);
        }
    }
}
