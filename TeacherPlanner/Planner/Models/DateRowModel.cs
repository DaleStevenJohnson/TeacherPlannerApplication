using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Planner.Models
{
    public class DateRowModel
    {
        public DateRowModel(DateTime date, bool week1 = true, bool week2 = false, bool holiday = false)
        {
            Date = date;
            Week1 = week1;
            Week2 = week2;
            Holiday = holiday;
        }
        public DateTime Date { get; }
        public string DateString => Date.ToString("yyyy/MM/dd");
        public bool Week1 { get; set; }
        public bool Week2 { get; set; }
        public bool Holiday { get; set; }
    }
}
