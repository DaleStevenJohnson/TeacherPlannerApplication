using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Planner.ViewModels
{
    public class KeyDateItemViewModel
    {
        public KeyDateItemViewModel()
        { 
            
        }

        public string Description { get; }
        public string Type { get; }
        public DateTime Date { get; }
        public string DateString { get; }
        public string TimeString { get; }
    }
}
