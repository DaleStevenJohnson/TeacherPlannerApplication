using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.ViewModels
{
    public class KeyDateItemViewModel
    {
        public KeyDateItemViewModel(string description, string keydatetype, DateTime date)
        {
            Description = description;
            Type = keydatetype;
            Date = date;
            IsChecked = false;
        }
        public bool IsChecked { get; set; }
        public string Description { get; }
        public string Type { get; }
        public DateTime Date { get; }
        public string DateString { get => Date.ToString("yyyy/MM/dd"); }
        public string TimeString { get => Date.ToString("HH:mm"); }

        public string GetProperty(string property)
        {
            return property.ToLower() switch
            {
                "description" => Description,
                "date" => DateString,
                "type" => Type,
                "time" => TimeString,
                _ => "Not Found",
            };
        }

        public override string ToString()
        {
            return $"{Description} - {Type} - {TimeString}";
        }

     


    }
}
