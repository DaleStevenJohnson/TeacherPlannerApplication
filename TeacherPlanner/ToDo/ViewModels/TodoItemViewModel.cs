using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.ToDo.Models;

namespace TeacherPlanner.ToDo.ViewModels
{

    public class TodoItemViewModel : ObservableObject
    {
        private bool _isChecked;

        public ICommand AddSubItemCommand { get; }
        public ICommand RemoveSelfCommand { get; set; }
        public event EventHandler<TodoItemViewModel> RemoveSelfEvent;

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
        }
        
        public string Content { get; set; }

        public bool IsChecked
        {
            get => _isChecked;
            set => RaiseAndSetIfChanged(ref _isChecked, value);
        }


        public void OnRemoveSelf()
        {
            RemoveSelfEvent.Invoke(null, this);
        }

        public ObservableCollection<TodoSubItemViewModel> SubItems { get; }      

        private void OnAddSubItem()
        {
            var item = new TodoSubItemViewModel();
            item.RemoveSelfEvent += (_, todoItem) => RemoveSubItem(todoItem);
            SubItems.Add(item);
        }

        private void RemoveSubItem(TodoSubItemViewModel item)
        {
            item.RemoveSelfEvent -= (_, todoItem) => RemoveSubItem(todoItem);
            SubItems.Remove(item);
        }
    }




}
