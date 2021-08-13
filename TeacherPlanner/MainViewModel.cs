using System;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.ViewModels;
using TeacherPlanner.Planner.Views;
using TeacherPlanner.ToDo.ViewModels;

namespace TeacherPlanner
{
    public class MainViewModel
    {
        public MainViewModel(UserModel userModel)
        {
            UserModel = userModel;
            PlannerViewModel = new PlannerViewModel(UserModel);
            ToDoViewModel = new ToDoViewModel(UserModel);
            ImportTimetableCommand = new SimpleCommand(_ => OnTimetableImportClick());
        }
        public UserModel UserModel;
        public PlannerViewModel PlannerViewModel { get; }
        public ToDoViewModel ToDoViewModel { get; }

        public ICommand ImportTimetableCommand { get;  }

        private void OnTimetableImportClick()
        {
            var importWindow = new ImportTimetableWindow();
            var importTimetableViewModel = new ImportTimetableWindowViewModel(UserModel);
            importWindow.DataContext = importTimetableViewModel;
            importWindow.ShowDialog();
        }
    }
}
