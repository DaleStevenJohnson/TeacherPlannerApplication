using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.ToDo.Models;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class TodoListViewModel : ObservableObject
    {
        

        public ICommand AddTodoItemCommand { get; }
        public TodoListViewModel(TodoListModel todoListModel)
        {
            Header = todoListModel.Header;

            TodoItems = new List<TodoItemViewModel>();

            foreach (var item in todoListModel.TodoItems)
            {
                TodoItems.Add(new TodoItemViewModel(item));
            }


            AddTodoItemCommand = new SimpleCommand(_ => AddTodoItem());
        }
        public string Header { get; }
        public TodoListModel TodoListModel { get; }
        public List<TodoItemViewModel> TodoItems { get; set; }
        

        public void AddTodoItem()
        {
            TodoItems.Add(new TodoItemViewModel());
        }
    }
}
