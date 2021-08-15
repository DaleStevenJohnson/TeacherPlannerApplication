using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Planner.Models
{
    public class TimetableModel
    {
        public TimetableModel(string[][] timetableFileData, string name)
        {
            Name = name;
            Create(timetableFileData);
        }
        public string Name { get; }
        public string[][][] Timetable { get; set; }
        private void Create(string[][] timetableFileData)
        {
            string[] days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            Timetable = new string[2][][];
            for (int row = 1; row < timetableFileData.Length; row++)
            {
            
                string[] periodArray = timetableFileData[row];
                int week = Int32.Parse(periodArray[0]) - 1;
                int day = Array.IndexOf(days, periodArray[1]);
                int period = Int32.Parse(periodArray[2]) - 1;
                string classcode = periodArray[6];
                string room = periodArray[7];
                Timetable[week][day][period] = classcode + "," + room;
            }
        }
        public string GetPeriod(int week, int day, int period)
        {
            return Timetable[week-1][day-1][period-1];
        }

    }
}
