using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.Models
{
    public class CalendarDateModel : ObservableObject
    {
        private bool _isKeyDate;

        //public event EventHandler<string> CalendarDayClickedEvent;
        public CalendarDateModel(string dayDate, int week, DateTime date)
        {
            DayDate = dayDate;
            Week = week;
            Date = date;
        }
        public DateTime Date;
        public string DayDate { get; }
        public int Week { get; }
        public bool IsDisplayedDate { get; set; }
        public bool IsKeyDate 
        {
            get => _isKeyDate;
            set => RaiseAndSetIfChanged(ref _isKeyDate, value);
        }
    }
}
