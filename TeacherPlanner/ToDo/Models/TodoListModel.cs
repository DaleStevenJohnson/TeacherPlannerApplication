using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Database.DatabaseModels;
using TeacherPlanner.ToDo.ViewModels;

namespace TeacherPlanner.ToDo.Models
{
    public class TodoListModel
    {
        public TodoListModel(string header, int id)
        {
            ID = id;
            Header = header;
            ActiveTodoItems = new ObservableCollection<TodoItemViewModel>();
            CompletedTodoItems = new ObservableCollection<TodoItemViewModel>();
        }
        public int ID { get; }
        public string Header { get; set; }
        public ObservableCollection<TodoItemViewModel> ActiveTodoItems { get; set; }
        public ObservableCollection<TodoItemViewModel> CompletedTodoItems { get; set; }
    }
}
