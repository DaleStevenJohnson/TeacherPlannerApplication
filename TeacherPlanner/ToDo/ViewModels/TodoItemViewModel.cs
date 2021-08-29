using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.ToDo.Models;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class TodoItemViewModel
    {
        public TodoItemViewModel(TodoItemModel todoItemModel)
        {
            Content = todoItemModel.Content;
            SubItems = new List<TodoSubItemViewModel>();

            foreach (var item in todoItemModel.SubItems)
            {
                SubItems.Add(new TodoSubItemViewModel(item));
            }
        }
        public TodoItemViewModel()
        {
            Content = "";
            SubItems = new List<TodoSubItemViewModel>();
        }
        public string Content { get; set; }
        public List<TodoSubItemViewModel> SubItems { get; set; }

        public void AddSubItem()
        {
            SubItems.Add(new TodoSubItemViewModel());
        }
    }




}
