using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Planner.ViewModels
{
    public class KeyDateItemViewModel
    {
        public KeyDateItemViewModel(string description, string keydatetype, DateTime date)
        {
            Description = description;
            Type = keydatetype;
            Date = date;
        }

        public string Description { get; }
        public string Type { get; }
        public DateTime Date { get; }
        public string DateString { get => Date.ToString("yyyy/MM/dd"); }
        public string TimeString { get => Date.ToString("HH:mm"); }

        public string GetProperty(string property)
        {
            switch (property.ToLower())
            {
                case "description":
                    return Description;
                case "date":
                    return DateString;
                case "type":
                    return Type;
                case "time":
                    return TimeString;
                default:
                    return "Not Found";
            }
        }
    }
}
