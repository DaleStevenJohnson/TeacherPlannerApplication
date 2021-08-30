using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class AddTodoListViewModel
    {
        public ICommand SubmitCommand { get; } 
        public AddTodoListViewModel(Window window )
        {
            Window = window;
            SubmitCommand = new SimpleCommand(_ => OnSubmit());
        }
        private Window Window { get; }
        public string TodoListName { get; set; }

        public void OnSubmit()
        {
            Window.DialogResult = true;
            Window.Close();
        }
    }
}
