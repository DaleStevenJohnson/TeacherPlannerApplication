﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
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
            Header = todoListModel.Header;

            TodoItems = new ObservableCollection<TodoItemViewModel>();
            RemoveSelfCommand = new SimpleCommand(_ => OnRemoveSelf());

            foreach (var item in todoListModel.TodoItems)
            {
                AddTodoItem(item);
            }


            AddTodoItemCommand = new SimpleCommand(_ => AddTodoItem());
        }
        public string Header { get; set; }
        public TodoListModel TodoListModel { get; }
        public ObservableCollection<TodoItemViewModel> TodoItems { get; set; }

        public void OnRemoveSelf()
        {
            RemoveSelfEvent.Invoke(null, this);
        }

        public void AddTodoItem(TodoItemModel item = null)
        {

            var newItem = item == null ? new TodoItemViewModel() : new TodoItemViewModel(item);
            newItem.RemoveSelfEvent += (_, todoItem) => RemoveTodoItem(todoItem);
            TodoItems.Add(newItem);
        }
        
        public void RemoveTodoItem(TodoItemViewModel item)
        {
            item.RemoveSelfEvent -= (_, todoItem) => RemoveTodoItem(todoItem);
            TodoItems.Remove(item);
        }
    }
}
