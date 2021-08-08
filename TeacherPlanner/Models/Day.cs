using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Helpers;



namespace TeacherPlanner.Models
{
    public class Day : ObservableObject
    {
        
        private string _date;
        private Period[] _periods;

        public Day(string date, int periods = 6)
        {
            Date = date;
            Periods = new Period[periods];
            LoadPeriods();
        }

        public Period[] Periods 
        {
            get => _periods;
            set => RaiseAndSet(ref _periods, value);
        }

        public string Date {
            get 
            {
                return _date;
            }
            set
            {
                _date = value;
            } 
        }

        private void LoadPeriods() 
        {
            for (int i = 0; i < Periods.Length; i++)
            {
                Random random = new Random();
                int year = random.Next(7, 13);
                string[] forms = new string[] { "A", "J", "P", "C" };
                int r = random.Next(0, 3);
                Periods[i] = new Period(i + 1, $"{year}{forms[r]}", Date, 7);
            }
        }
    }
}
