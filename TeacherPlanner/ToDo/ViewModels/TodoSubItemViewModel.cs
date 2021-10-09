using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Database;
using Database.DatabaseModels;
using TeacherPlanner.Helpers;
using TeacherPlanner.ToDo.Models;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class TodoSubItemViewModel : ObservableObject
    { 

        // Events & Commands
        public event EventHandler<TodoSubItemViewModel> RemoveSelfEvent;
        
        public ICommand OnCheckedCommand { get; set; }
        public ICommand RemoveSelfCommand { get; set; }

        public TodoSubItemViewModel(TodoSubItemModel subItemModel)
        {
            Model = subItemModel;

            RemoveSelfCommand = new SimpleCommand(_ => OnRemoveSelf());
            Model.ValuesUpdatedEvent += (_, __) => UpdateDatabase();
        }

        // Properties

        public TodoSubItemModel Model { get; }

        public string Content { get; set; }


        private void UpdateDatabase()
        {
            var dbModel = new TodoItem()
            {
                ID = Model.ID,
                IsSubItem = true,
                Description = Model.Content,
                IsCompleted = Model.IsChecked,
                
                TodoListID = 0,
                ParentItem = null,
            };

            DatabaseManager.TryUpdateTodoItem(dbModel);
        }


        public void OnRemoveSelf()
        {
            Model.ValuesUpdatedEvent -= (_, __) => UpdateDatabase();
            RemoveSelfEvent.Invoke(null, this);
        }
    }
}
