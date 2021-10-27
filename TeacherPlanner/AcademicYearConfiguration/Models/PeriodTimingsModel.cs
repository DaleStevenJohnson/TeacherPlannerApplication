using System;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.AcademicYearConfiguration.Models
{
    public class PeriodTimingsModel : ObservableObject
    {
        public PeriodTimingsModel()
        {
            StartTimeHour = ComboBoxLists.HoursList[0];
            StartTimeMinute = ComboBoxLists.MinuteList[0];
            EndTimeHour = ComboBoxLists.HoursList[0];
            EndTimeMinute = ComboBoxLists.MinuteList[0];
        }
        public PeriodCodes PeriodCode { get; set; }
        public string StartTimeHour { get; set; }
        public string StartTimeMinute { get; set; }
        public string EndTimeHour { get; set; }
        public string EndTimeMinute { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
