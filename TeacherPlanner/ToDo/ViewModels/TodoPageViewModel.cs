using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.ToDo.Models;
using System.Linq;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using TeacherPlanner.ToDo.Views;
using System.Windows;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class TodoPageViewModel : ObservableObject
    {

        private ObservableCollection<TodoListViewModel> _todoLists;
        private bool _canEditListNames;
        private bool _canDeleteAnything;
        private string _newListName;
        private bool _isAddingNewList;

        public ICommand AddTodoListCommand { get; }
        public ICommand ConfirmNewTodoListCommand { get; }
        public ICommand CancelNewTodoListCommand { get; }
        public ICommand RemoveTodoListCommand { get; }
        public TodoPageViewModel(UserModel userModel)
        {
            UserModel = userModel;

            TodoLists = new ObservableCollection<TodoListViewModel>();
            CanEditListNames = false;
            CanDeleteAnything = false;
            IsAddingNewList = false;

            AddTodoListCommand = new SimpleCommand(_ => OnAddTodoList());
            RemoveTodoListCommand = new SimpleCommand(todoListModel => OnRemoveTodoList((TodoListViewModel)todoListModel));
            ConfirmNewTodoListCommand = new SimpleCommand(_ => OnConfirmNewList());
            CancelNewTodoListCommand = new SimpleCommand(_ => ResetAddNewListBox());
        }

        public string NewListName
        {
            get => _newListName;
            set => RaiseAndSetIfChanged(ref _newListName, value);
        }

        public bool IsAddingNewList
        { 
            get => _isAddingNewList; 
            set => RaiseAndSetIfChanged(ref _isAddingNewList, value);
        }   
    public UserModel UserModel { get; }
        public bool CanEditListNames 
        { 
            get => _canEditListNames; 
            set => RaiseAndSetIfChanged(ref _canEditListNames, value);
        }

        public bool CanDeleteAnything
        {
            get => _canDeleteAnything;
            set => RaiseAndSetIfChanged(ref _canDeleteAnything, value);
        }

        public ObservableCollection<TodoListViewModel> TodoLists
        {
            get => _todoLists;
            set => RaiseAndSetIfChanged(ref _todoLists, value);
        }

        
        private void OnRemoveTodoList(TodoListViewModel todoListModel)
        {
            if (todoListModel.ActiveTodoItems.Any())
            {
                var result = MessageBox.Show(
                    "This List still has active tasks - are you sure you want to delete?",
                    "Confirm Delete List",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                    return;
            }
            todoListModel.RemoveSelfEvent -= (_, __) => OnRemoveTodoList(__);
            TodoLists.Remove(todoListModel);
        }
        public void OnConfirmNewList()
        {
            if (NewListName.Length > 0)
            {
                var list = new TodoListViewModel(new TodoListModel(NewListName));
                list.RemoveSelfEvent += (_, __) => OnRemoveTodoList(__);
                TodoLists.Add(list);
                ResetAddNewListBox();
            }
        }

        private void ResetAddNewListBox()
        {
            IsAddingNewList = false;
            NewListName = "";
        }

        private void OnAddTodoList()
        {
            IsAddingNewList = true;
        }
    }
}
