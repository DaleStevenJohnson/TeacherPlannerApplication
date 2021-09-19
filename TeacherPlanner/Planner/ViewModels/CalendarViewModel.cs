using System;
using System.Collections.ObjectModel;
using System.Linq;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Planner.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class CalendarViewModel : ObservableObject
    {
        private readonly int CALENDARSPACES = 42;
        private ObservableCollection<KeyDateItemViewModel> _keyDates;

        public CalendarViewModel(DateTime date, ObservableCollection<KeyDateItemViewModel> keydates)
        {
            Date = date;
            KeyDates = keydates;
            Dates = new CalendarDateModel[CALENDARSPACES];
            PopulateDates();
        }

        // Properties
        public CalendarDateModel[] Dates { get; set; }
        public string Name => Date.ToString(Formats.FullDayNameFormat); 
        public string Month => Date.ToString(Formats.FullMonthNameFormat);
        public string Year => Date.ToString(Formats.FullYearNumberFormat);
        public string DisplayDate { get { return Date.ToString(Formats.DateHeadingFormat); } }
        public string DisplayMonthYear { get { return Date.ToString(Formats.FullMonthNameAndFullYearNumberFormat); } }
        public DateTime Date { get; set; }
        private ObservableCollection<KeyDateItemViewModel> KeyDates
        {
            get => _keyDates;
            set => RaiseAndSetIfChanged(ref _keyDates, value);
        }

        // Methods
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
                //var week = CalendarManager.GetWeek(date);
                var week = 1;
                if (i >= firstDay && i < firstDay + daysInMonth)
                {
                    var calendarDateModel = new CalendarDateModel(currentDayOfMonth.ToString(), week, date);
                    if (date == Date)
                        calendarDateModel.IsDisplayedDate = true;
                    Dates[i] = calendarDateModel;
                    currentDayOfMonth++;
                    date = date.AddDays(1);
                }
                else
                    // Setting week to 99 as is is a blank week and do not want it to inherit any formatting
                    Dates[i] = new CalendarDateModel("", 99, DateTime.MinValue);
            }
        }

        public void SetKeyDates()
        {
            if (KeyDates != null)
            {
                foreach (CalendarDateModel date in Dates)
                {
                    date.IsKeyDate = KeyDates.Any(kd => kd.Date.Date == date.Date.Date);
                }
            }
        }
    }
}
