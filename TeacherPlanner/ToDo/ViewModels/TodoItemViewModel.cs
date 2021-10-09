using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Database;
using Database.DatabaseModels;
using TeacherPlanner.Helpers;
using TeacherPlanner.ToDo.Models;

namespace TeacherPlanner.ToDo.ViewModels
{

    public class TodoItemViewModel : ObservableObject
    {
        private bool _hasActiveSubItems;

        public ICommand AddSubItemCommand { get; }
        public ICommand RemoveSelfCommand { get; set; }
        
        public event EventHandler<TodoItemViewModel> RemoveSelfEvent;
        public event EventHandler<TodoItemViewModel> CompletedStatusChangedEvent;

        public TodoItemViewModel(TodoItemModel todoItemModel)
        {
            // Set properties to default
            Model = todoItemModel;

            AddSubItemCommand = new SimpleCommand(_ => OnAddSubItem());
            RemoveSelfCommand = new SimpleCommand(_ => OnRemoveSelf());

            Model.ValuesUpdatedEvent += (_, __) => OnTodoItemUpdated();
            Model.CompletedStatusChangedEvent += (_, __) => OnCompletedStatusChanged();
            SetHasNoActiveSubItems();
        }

        // Properties
        public TodoItemModel Model { get; }

        public bool HasActiveSubItems
        {
            get => _hasActiveSubItems;
            set => RaiseAndSetIfChanged(ref _hasActiveSubItems, value);
        }

        // Methods
        public void OnRemoveSelf()
        {
            Model.ValuesUpdatedEvent -= (_, __) => OnTodoItemUpdated();
            RemoveSelfEvent.Invoke(null, this);
        }

        public void RemoveAllSubItems()
        {
            var newList = new List<TodoSubItemViewModel>(Model.SubItems);
            foreach (var item in newList)
            {
                RemoveSubItem(item);
            }
        }

        public void GetSubItems(IEnumerable<TodoItem> items)
        {
            foreach (var subitem in items)
            {
                var subitemModel = new TodoSubItemModel(subitem.Description, subitem.IsCompleted, subitem.ID);
                var subitemViewModel = new TodoSubItemViewModel(subitemModel);

                subitemViewModel.RemoveSelfEvent += (_, todoItem) => RemoveSubItem(todoItem);
                subitemViewModel.Model.ValuesUpdatedEvent += (_, __) => SetHasNoActiveSubItems();

                Model.SubItems.Add(subitemViewModel);
            }
        }


        public void RemoveSubItem(TodoSubItemViewModel item)
        {
            if (DatabaseManager.RemoveTodoItem(item.Model.ID))
            {
                item.RemoveSelfEvent -= (_, todoItem) => RemoveSubItem(todoItem);
                Model.SubItems.Remove(item);
                SetHasNoActiveSubItems();
            }
        }


        public void OnTodoItemUpdated()
        {
            UpdateDatabase();
            SetHasNoActiveSubItems();
            
        }

        public void SetHasNoActiveSubItems()
        {
            HasActiveSubItems = Model.SubItems.Any(i => i.Model.IsChecked == false);
        }

        private void OnCompletedStatusChanged()
        {
            CompletedStatusChangedEvent.Invoke(null, this);
        }

        private void OnAddSubItem()
        {
            var dbModel = new TodoItem()
            {
                TodoListID = Model.ListID,
                IsSubItem = true,
                ParentItem = Model.ID,
                Description = string.Empty,
                IsCompleted = false
            };

            if (DatabaseManager.TryAddTodoItem(dbModel, out var id))
            {
                var model = new TodoSubItemModel(string.Empty, false, id);
                var item = new TodoSubItemViewModel(model);
                item.RemoveSelfEvent += (_, todoItem) => RemoveSubItem(todoItem);
                item.Model.ValuesUpdatedEvent += (_, __) => SetHasNoActiveSubItems();
                Model.SubItems.Add(item);
                SetHasNoActiveSubItems();
            }
        }

        

        private void UpdateDatabase()
        {
            var dbModel = new TodoItem()
            {
                ID = Model.ID,
                TodoListID = Model.ListID,
                IsSubItem = false,
                ParentItem = null,
                Description = Model.Content,
                IsCompleted = Model.IsChecked,
            };

            DatabaseManager.TryUpdateTodoItem(dbModel);
        }

        
    }
}
