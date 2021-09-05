using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.ViewModels
{
    public class KeyDateItemViewModel
    {
        public ICommand RemoveSelfCommand { get; }
        public event EventHandler<KeyDateItemViewModel> RemoveSelfEvent;
        public KeyDateItemViewModel(string description, string keydatetype, DateTime date)
        {
            Description = description;
            Type = keydatetype;
            Date = date;

            RemoveSelfCommand = new SimpleCommand(_ => OnRemoveSelf());
        }

        public string Description { get; }
        public string Type { get; }
        private DateTime Date { get; }
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

        private void OnRemoveSelf()
        {
            RemoveSelfEvent.Invoke(null, this);
        }
    }
}
