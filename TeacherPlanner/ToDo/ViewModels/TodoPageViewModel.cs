using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.ToDo.Models;
using System.Linq;
using System.Windows.Controls;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class TodoPageViewModel : ObservableObject
    {
        private List<TodoListModel> _todoListModels;
        private List<TodoListViewModel> _todoListViewModels;

        public ICommand AddTodoListCommand { get; }
        public ICommand RemoveTodoListCommand { get; }
        public TodoPageViewModel(UserModel userModel)
        {
            UserModel = userModel;
            
            TodoLists = new List<TodoListViewModel>();

            AddTodoListCommand = new SimpleCommand(_ => OnAddTodoList());
            RemoveTodoListCommand = new SimpleCommand(todoListModel => OnRemoveTodoList((TodoListViewModel)todoListModel));


            for (int i = 0; i < 3; i++)
            {
                TodoLists.Add(new TodoListViewModel(new TodoListModel(i.ToString())));
            }
        }

        public UserModel UserModel { get; }
        
        public List<TodoListViewModel> TodoLists
        {
            get => _todoListViewModels;
            set => RaiseAndSetIfChanged(ref _todoListViewModels, value);
        }

        private void OnAddTodoList()
        {
            TodoLists.Add(new TodoListViewModel(new TodoListModel("")));
             
        }
        private void OnRemoveTodoList(TodoListViewModel todoListModel)
        {
            TodoLists.Remove(todoListModel);
        }

    }
}
