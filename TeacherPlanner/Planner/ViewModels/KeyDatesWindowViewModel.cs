using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TeacherPlanner.Planner.ViewModels
{
    public class KeyDatesWindowViewModel
    {
        public KeyDatesWindowViewModel()
        {
            return;
        }

        public ObservableCollection<KeyDateItemViewModel> KeyDates { get; }
    }
}
