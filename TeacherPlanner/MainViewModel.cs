using System.Collections.Generic;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.Planner.ViewModels;
using TeacherPlanner.Timetable.ViewModels;
using TeacherPlanner.ToDo.ViewModels;

namespace TeacherPlanner
{
    public class MainViewModel : ObservableObject
    {
        private int _currentView;
        public ICommand ChoosePlannerYearCommand { get; }
        public MainViewModel(UserModel userModel)
        {
            UserModel = userModel;
            CurrentView = 1;
            ChoosePlannerYearCommand = new SimpleCommand(yearString => OnChoosePlannerYear((string)yearString));
            ChooseYearViewModel = new ChooseYearViewModel(UserModel);
            ChooseYearViewModel.ChooseYearEvent += (_,__) => OnChoosePlannerYear(__);
        }
        public int CurrentView 
        {
            get => _currentView;
            set => RaiseAndSetIfChanged(ref _currentView, value);
        }
        public ChooseYearViewModel ChooseYearViewModel { get; set; }
        public PlannerYearViewModel PlannerYearViewModel { get; set; }
        public UserModel UserModel;
        private void OnChoosePlannerYear(object parameter)
        {
            var yearString = (string)parameter;
            PlannerYearViewModel = new PlannerYearViewModel(UserModel, yearString);
            SwitchView();
        }
        public void SwitchView()
        {
            CurrentView = CurrentView == 1 ? 2 : 1;
        }
        
    }
}
