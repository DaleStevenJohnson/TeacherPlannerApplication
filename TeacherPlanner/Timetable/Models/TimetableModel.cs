using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetableModel
    {
        public TimetableModel(string[][] timetableFileData)
        {
            Create(timetableFileData);
        }
        
        public TimetableWeekModel Week1 { get; private set; }
        public TimetableWeekModel Week2 { get; private set; }
        private void Create(string[][] timetableFileData)
        {
            string[] days = new string[] {"", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            //Dictionary<string, int> classCodeCounts = new Dictionary<string, int>();
            // 5 Days
            // 8 Periods - 5 teaching, two breaks + registration
            // 4 items of data per period
            Week1 = new TimetableWeekModel("Week 1");
            Week2 = new TimetableWeekModel("Week 2");

            for (var row = 1; row < timetableFileData.Length; row++)
            {
                string[] periodArray = timetableFileData[row];
                var week = Int32.Parse(periodArray[0]);
                var day = Array.IndexOf(days, periodArray[1]);
                var period = periodArray[2];
                
                // Setting period by reference
                TimetablePeriodModel periodModel = GetPeriod(week, day, period);
                periodModel.ClassCode = periodArray[4];
                periodModel.Room = periodArray[5];
                periodModel.Occurance = periodArray[6];
                periodModel.Occurances = periodArray[7];

            }
        }
        
        public TimetablePeriodModel GetPeriod(int week, int day, string period)
        {
            return week == 1 ? Week1.GetPeriod(day, period) : Week2.GetPeriod(day, period);
        }

       
    }
}
