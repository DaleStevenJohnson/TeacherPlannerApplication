using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.ToDo.Models
{
    public class TodoItemModel
    {
        public TodoItemModel()
        {
            Content = string.Empty;
            SubItems = new List<TodoSubItemModel>
            {
                new TodoSubItemModel() { Content = "Bob"},
                new TodoSubItemModel() { Content = "Steve"},
                new TodoSubItemModel() { Content = "Eric"}
            };
            IsChecked = false;
        }
        public bool IsChecked { get; }
        public string Content { get; set; }
        public List<TodoSubItemModel> SubItems { get; set; }
    }
}
