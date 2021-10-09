using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Database.DatabaseModels;
using TeacherPlanner.Helpers;
using TeacherPlanner.ToDo.ViewModels;

namespace TeacherPlanner.ToDo.Models
{
    public class TodoListModel : ObservableObject
    {
        private string _header;

        public event EventHandler ValuesUpdatedEvent;
        public TodoListModel(string header, int id)
        {
            ID = id;
            Header = header;
            ActiveTodoItems = new ObservableCollection<TodoItemViewModel>();
            CompletedTodoItems = new ObservableCollection<TodoItemViewModel>();
        }
        public int ID { get; }
        public string Header 
        {
            get => _header;
            set
            {
                if (RaiseAndSetIfChanged(ref _header, value) && ValuesUpdatedEvent != null)
                    ValuesUpdatedEvent.Invoke(null, EventArgs.Empty);
            }
        }
        public ObservableCollection<TodoItemViewModel> ActiveTodoItems { get; set; }
        public ObservableCollection<TodoItemViewModel> CompletedTodoItems { get; set; }
    }
}
