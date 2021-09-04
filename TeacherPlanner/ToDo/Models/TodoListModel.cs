using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.ToDo.Models
{
    public class TodoListModel
    {
        private List<TodoItemModel> _todoItems;
        public TodoListModel(string header)
        {
            Header = header;
            TodoItems = new List<TodoItemModel>
            {
                new TodoItemModel(){Content = "Item 1"},
                new TodoItemModel(){Content = "Item 2"},
                new TodoItemModel(){Content = "Item 3"}
            };
        }
        public string Header { get; set; }
        public List<TodoItemModel> TodoItems { get; set; }
    }
}
