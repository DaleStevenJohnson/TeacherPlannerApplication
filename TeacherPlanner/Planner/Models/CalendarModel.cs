using System;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.Models
{
    public class CalendarModel
    {
        private readonly int CALENDARSPACES = 42;
        public CalendarModel(DateTime date)
        {
            Date = date;
            Dates = new string[CALENDARSPACES];
            PopulateDates();
        }
        public string[] Dates { get; set; }
        public string Name => Date.ToString(Formats.FullDayNameFormat); 
        public string Month => Date.ToString(Formats.FullMonthNameFormat);
        public string Year => Date.ToString(Formats.FullYearNumberFormat);
        public string DisplayDate { get { return Date.ToString(Formats.DateHeadingFormat); } }
        public string DisplayMonthYear { get { return Date.ToString(Formats.FullMonthNameAndFullYearNumberFormat); } }
        public DateTime Date { get; set; }
        public void PopulateDates()
        {
            var yearInt = Int32.Parse(Date.ToString(Formats.FullYearNumberFormat));
            var monthInt = Int32.Parse(Date.ToString(Formats.FullMonthNumberFormat));
            var days = DateTime.DaysInMonth(yearInt, monthInt);
            var firstDate = new DateTime(yearInt, monthInt, 1);
            var firstDay = (int)firstDate.DayOfWeek;
            firstDay = firstDay == 0 ? 6 : firstDay - 1;
            var day = 1;
            for (int i = 0; i < CALENDARSPACES; i++)
            {
                if (i >= firstDay && i <= days)
                {
                    Dates[i] = day.ToString();
                    day += 1;
                }
                else
                    Dates[i] = "";
            }
            
        }
    }
}
