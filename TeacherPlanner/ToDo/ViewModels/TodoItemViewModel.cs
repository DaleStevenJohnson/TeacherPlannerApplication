using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.ToDo.Models;

namespace TeacherPlanner.ToDo.ViewModels
{

    public class TodoItemViewModel : ObservableObject
    {
        private bool _isChecked;
        private bool _hasActiveSubItems;

        public ICommand AddSubItemCommand { get; }
        public ICommand RemoveSelfCommand { get; set; }
        public event EventHandler<TodoItemViewModel> RemoveSelfEvent;
        public event EventHandler<TodoItemViewModel> CompletedStatusChangedEvent;

        public TodoItemViewModel(TodoItemModel todoItemModel = null)
        {
            SubItems = new ObservableCollection<TodoSubItemViewModel>();

            // Set properties to default
            Content = "";
            IsChecked = false;
            

            AddSubItemCommand = new SimpleCommand(_ => OnAddSubItem());
            RemoveSelfCommand = new SimpleCommand(_ => OnRemoveSelf());

            // If passed a model, populate properties from model
            if (todoItemModel != null)
            {
                Content = todoItemModel.Content;
                IsChecked = todoItemModel.IsChecked;
                foreach (var item in todoItemModel.SubItems)
                {
                    OnAddSubItem();
                }
            }
            SetHasNoActiveSubItems();
        }

        // Properties

        public string Content { get; set; }
        public bool HasActiveSubItems 
        {
            get => _hasActiveSubItems;
            set => RaiseAndSetIfChanged(ref _hasActiveSubItems, value);
        }   

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (RaiseAndSetIfChanged(ref _isChecked, value))
                    CompletedStatusChangedEvent.Invoke(null, this);
            }
        }

        public ObservableCollection<TodoSubItemViewModel> SubItems { get; }

        // Methods

        public void OnRemoveSelf()
        {
            RemoveSelfEvent.Invoke(null, this);
        }

           

        private void OnAddSubItem()
        {
            var item = new TodoSubItemViewModel();
            item.RemoveSelfEvent += (_, todoItem) => RemoveSubItem(todoItem);
            item.CheckedEvent += (_, __) => SetHasNoActiveSubItems();
            SubItems.Add(item);
            SetHasNoActiveSubItems();
        }

        private void SetHasNoActiveSubItems()
        {
            HasActiveSubItems = SubItems.Any(i => i.IsChecked == false);
        }

        private void RemoveSubItem(TodoSubItemViewModel item)
        {
            item.RemoveSelfEvent -= (_, todoItem) => RemoveSubItem(todoItem);
            item.CheckedEvent -= (_, __) => SetHasNoActiveSubItems();
            SubItems.Remove(item);
            SetHasNoActiveSubItems();
        }
    }




}
