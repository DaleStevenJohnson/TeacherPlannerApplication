using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Login.Models;

namespace TeacherPlanner.ViewModels
{
    public class ToDoViewModel
    {
        public ToDoViewModel(UserModel userModel)
        {
            UserModel = userModel;
        }
        public UserModel UserModel;
    }
}
