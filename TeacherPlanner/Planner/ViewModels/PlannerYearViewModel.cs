using System;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Timetable.ViewModels;
using TeacherPlanner.ToDo.ViewModels;

namespace TeacherPlanner.Planner.ViewModels
{
    public class PlannerYearViewModel
    {
        public ICommand DefineTimetableWeeksCommand { get; }
        public event EventHandler<string> SwitchViewEvent;
        public ICommand SwitchViewCommand { get; }
        public PlannerYearViewModel(UserModel userModel, string yearString)
        {
            Year = yearString;
            UserModel = userModel;
            FileHandlingHelper.SetDirectories(UserModel.UsernameHash, Year);
            
            TimetableViewModel = new TimetableViewModel(UserModel);
            PlannerViewModel = new PlannerViewModel(UserModel, TimetableViewModel.CurrentTimetable);
            ToDoViewModel = new ToDoViewModel(UserModel);
            
            DefineTimetableWeeksCommand = new SimpleCommand(_ => OnDefineTimetableWeeks());
            SwitchViewCommand = new SimpleCommand(_ => OnSwitchView(_));
            
        }
        public string Year { get; }
        public UserModel UserModel;
        public PlannerViewModel PlannerViewModel { get; }
        public ToDoViewModel ToDoViewModel { get; }
        public TimetableViewModel TimetableViewModel { get; }
        private void OnDefineTimetableWeeks()
        {
            if (TimetableViewModel.DefineTimetableWeeks() ?? true)
                PlannerViewModel.LoadNewDays(true);
        }
        private void OnSwitchView(object v)
        {
            SwitchViewEvent.Invoke(null, (string)v);
        }
    }
}
