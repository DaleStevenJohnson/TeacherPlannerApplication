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
            RemoveTodoListCommand = new SimpleCommand(todoListModel => OnRemoveTodoList((TodoListModel)todoListModel));


            for (int i = 0; i < 3; i++)
            {
                TodoLists.Add(new TodoListViewModel(i.ToString()));
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
            TodoListModels.Add(new TodoListModel(""));
             
        }
        private void OnRemoveTodoList(TodoListModel todoListModel)
        {
            TodoListModels.Remove(todoListModel);
        }

    }
}
