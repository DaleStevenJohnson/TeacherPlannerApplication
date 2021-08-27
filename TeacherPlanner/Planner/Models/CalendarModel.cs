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
            Dates = new CalendarDateModel[CALENDARSPACES];
            PopulateDates();
        }
        public CalendarDateModel[] Dates { get; set; }
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
            var daysInMonth = DateTime.DaysInMonth(yearInt, monthInt);
            var firstDate = new DateTime(yearInt, monthInt, 1);
            var firstDay = (int)firstDate.DayOfWeek;
            firstDay = firstDay == 0 ? 6 : firstDay - 1;
            var date = new DateTime(yearInt, monthInt, 1);
            for (int i = 0, currentDayOfMonth = 1; i < CALENDARSPACES; i++)
            {
                var week = CalendarManager.GetWeek(date);
                if (i >= firstDay && i < firstDay + daysInMonth)
                {
                    var calendarDateModel = new CalendarDateModel(currentDayOfMonth.ToString(), week);
                    if (date == Date)
                        calendarDateModel.IsDisplayedDate = true;
                    Dates[i] = calendarDateModel;
                    currentDayOfMonth++;
                    date = date.AddDays(1);
                }
                else
                    // Setting week to 99 as is is a blank week and do not want it to inherit any formatting
                    Dates[i] = new CalendarDateModel("", 99);
                
            }
            
        }
    }
}
