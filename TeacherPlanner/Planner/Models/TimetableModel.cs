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
            Timetable = Initialise3DArray(2, 5, 6);
            for (int row = 1; row < timetableFileData.Length; row++)
            {
                string[] periodArray = timetableFileData[row];
                int week = Int32.Parse(periodArray[0]) - 1;
                int day = Array.IndexOf(days, periodArray[1]);
                int period = Int32.Parse(periodArray[2]) - 1;
                string classcode = periodArray[4];
                string room = periodArray[5];
                Timetable[week][day][period] = classcode; //+ "," + room;
            }
        }
        private string[][][] Initialise3DArray(int rows, int columns, int items)
        {
            string[][][] new3D = new string[2][][];
            for (int row = 0; row < rows; row++)
            {
                string[][] new2D = new string[columns][];
                for (int column = 0; column < columns; column++)
                {
                    new2D[column] = new string[items];
                }
                new3D[row] = new2D;
            }
            return new3D;
        }
        public string GetPeriod(int week, int day, int period)
        {
            return Timetable[week-1][day-1][period-1];
        }

    }
}
