using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.Models
{
    public class CalendarDateModel : ObservableObject
    {
        private bool _isKeyDate = false;
        private bool _isDeadline = false;
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

        public bool IsDeadline
        {
            get => _isDeadline;
            set => RaiseAndSetIfChanged(ref _isDeadline, value);
        }
    }
}
