using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Login.Models;

namespace TeacherPlanner.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel(UserModel userModel)
        {
            UserModel = userModel;
            PlannerViewModel = new PlannerViewModel(UserModel);
            ToDoViewModel = new ToDoViewModel(UserModel);
        }
        public UserModel UserModel;
        public PlannerViewModel PlannerViewModel { get; }
        public ToDoViewModel ToDoViewModel { get; }
    }
}
