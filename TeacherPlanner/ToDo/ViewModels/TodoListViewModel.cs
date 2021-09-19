using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Database;
using Database.DatabaseModels;
using TeacherPlanner.Helpers;
using TeacherPlanner.ToDo.Models;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class TodoListViewModel : ObservableObject
    {

        public ICommand AddTodoItemCommand { get; }
        public ICommand RemoveSelfCommand { get; set; }
        public event EventHandler<TodoListViewModel> RemoveSelfEvent;

        public TodoListViewModel(TodoListModel todoListModel)
        {
            // Property Assignment
            Model = todoListModel;

            // Command Assignment
            RemoveSelfCommand = new SimpleCommand(_ => OnRemoveSelf());
            AddTodoItemCommand = new SimpleCommand(_ => AddTodoItem());

            Model.ValuesUpdatedEvent += (_, __) => UpdateDatabase();

            GetTodoItems();
        }

        // Properties
        public TodoListModel Model { get; }

        public void OnRemoveSelf()
        {
            RemoveSelfEvent.Invoke(null, this);
        }

        public void AddTodoItem()
        {
            var dbModel = new TodoItem()
            {
                TodoListID = Model.ID,
                IsSubItem = false,
                ParentItem = null,
                Description = string.Empty,
                IsCompleted = false
            };
            
            if (DatabaseManager.TrySaveTodoItem(dbModel, out var id))
            {
                var model = new TodoItemModel(string.Empty, false, id, Model.ID);
                var newItem = new TodoItemViewModel(model);

                newItem.RemoveSelfEvent += (_, todoItem) => RemoveTodoItem(todoItem);

                newItem.CompletedStatusChangedEvent += (_, todoItem) => OnTodoItemCompleted(todoItem);
                OnTodoItemCompleted(newItem);
            }
        }

        
        
        public void RemoveTodoItem(TodoItemViewModel item)
        {
            if (item.Model.SubItems.Any())
            {
                var result = MessageBox.Show(
                    "This todo task has sub-tasks - are you sure you want to delete?",
                    "Confirm Delete Task",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                    return;
            }

            item.RemoveAllSubItems();

            if (DatabaseManager.RemoveTodoItem(item.Model.ID))
            {
                item.RemoveSelfEvent -= (_, todoItem) => RemoveTodoItem(todoItem);
                item.CompletedStatusChangedEvent -= (_, todoItem) => OnTodoItemCompleted(todoItem);
                Model.ActiveTodoItems.Remove(item);
            }
        }


        private void GetTodoItems()
        {
            var items = DatabaseManager.GetTodoItems(Model.ID);
            foreach (var item in items.Where(i => !i.IsSubItem))
            {
                var model = new TodoItemModel(item.Description, item.IsCompleted, item.ID, Model.ID);
                var viewmodel = new TodoItemViewModel(model);
                
                viewmodel.RemoveSelfEvent += (_, todoItem) => RemoveTodoItem(todoItem);
                viewmodel.CompletedStatusChangedEvent += (_, todoItem) => OnTodoItemCompleted(todoItem);

                viewmodel.GetSubItems(items.Where(si => si.ParentItem == model.ID));

                OnTodoItemCompleted(viewmodel);
            }
        }

        private void UpdateDatabase()
        {
            var dbModel = new TodoList()
            {
                ID = Model.ID,
                Name = Model.Header,
                AcademicYearID = 0,
            };

            DatabaseManager.TryUpdateTodoList(dbModel);
        }

        private void OnTodoItemCompleted(TodoItemViewModel item)
        {
            if (item.Model.IsChecked)
            {
                Model.ActiveTodoItems.Remove(item);
                Model.CompletedTodoItems.Add(item);
            }
            else
            {
                Model.CompletedTodoItems.Remove(item);
                Model.ActiveTodoItems.Add(item);
            }
        }
    }

    
}
