using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.ToDo.Models
{
    public class TodoSubItemModel
    {
        public TodoSubItemModel()
        {
            Content = string.Empty;
            IsChecked = true;
        }
        public bool IsChecked { get; }
        public string Content { get; set; }
    }
}
