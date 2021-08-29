using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.ToDo.Models;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class TodoSubItemViewModel
    {
        public TodoSubItemViewModel(TodoSubItemModel subItemModel)
        {
            Content = subItemModel.Content;
        }
        public TodoSubItemViewModel()
        {
            Content = "";
        }

        public string Content { get; set; }

    }
}
