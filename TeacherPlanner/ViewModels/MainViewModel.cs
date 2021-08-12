using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            PageViewModel = new PageViewModel();
            ToDoViewModel = new ToDoViewModel();
        }

        public PageViewModel PageViewModel { get; }
        public ToDoViewModel ToDoViewModel { get; }
    }
}
