﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.ToDo.Models;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class TodoSubItemViewModel : ObservableObject
    { 
        private bool _isChecked;
        public event EventHandler<TodoSubItemViewModel> RemoveSelfEvent;
        public event EventHandler CheckedEvent;
        public ICommand OnCheckedCommand { get; set; }
        public ICommand RemoveSelfCommand { get; set; }
        public TodoSubItemViewModel(TodoSubItemModel subItemModel = null)
        {
            Content = "";
            IsChecked = false;

            RemoveSelfCommand = new SimpleCommand(_ => OnRemoveSelf());

            if (subItemModel != null)
            {
                Content = subItemModel.Content;
                IsChecked = subItemModel.IsChecked;
            }
        }

        public string Content { get; set; }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (RaiseAndSetIfChanged(ref _isChecked, value))
                    CheckedEvent.Invoke(null, null);
            }
        }

        public void OnRemoveSelf()
        {
            RemoveSelfEvent.Invoke(null, this);
        }
    }
}
