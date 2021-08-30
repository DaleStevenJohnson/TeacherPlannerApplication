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

namespace TeacherPlanner.ToDo.ViewModels
{
    public class TodoPageViewModel : ObservableObject
    {

        private ObservableCollection<TodoListViewModel> _todoLists;
        private bool _canEditListNames;
        private bool _canDeleteAnything;

        public ICommand AddTodoListCommand { get; }
        public ICommand RemoveTodoListCommand { get; }
        public TodoPageViewModel(UserModel userModel)
        {
            UserModel = userModel;

            TodoLists = new ObservableCollection<TodoListViewModel>();
            CanEditListNames = false;
            CanDeleteAnything = false;

            AddTodoListCommand = new SimpleCommand(_ => OnAddTodoList());
            RemoveTodoListCommand = new SimpleCommand(todoListModel => OnRemoveTodoList((TodoListViewModel)todoListModel));
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

        private void OnAddTodoList()
        {

            var window = new AddTodoListWindow();
            var viewmodel = new AddTodoListViewModel(window);
            window.DataContext = viewmodel;
            
            if (window.ShowDialog() ?? true)
            {
                var list = new TodoListViewModel(new TodoListModel(viewmodel.TodoListName));
                list.RemoveSelfEvent += (_, __) => OnRemoveTodoList(__);
                TodoLists.Add(list);
            }

        }
        private void OnRemoveTodoList(TodoListViewModel todoListModel)
        {
            todoListModel.RemoveSelfEvent -= (_, __) => OnRemoveTodoList(__);
            TodoLists.Remove(todoListModel);
        }

    }
}
