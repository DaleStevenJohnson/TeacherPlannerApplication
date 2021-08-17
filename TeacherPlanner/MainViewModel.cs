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
        PlannerYearViewModel _plannerYearViewModel;
        public ICommand ChoosePlannerYearCommand { get; }
        public ICommand SwitchViewCommand { get;  }
        public MainViewModel(UserModel userModel)
        {
            UserModel = userModel;
            CurrentView = 1;
            ChoosePlannerYearCommand = new SimpleCommand(yearString => OnChoosePlannerYear((string)yearString));
            SwitchViewCommand = new SimpleCommand(_ => SwitchView());
            ChooseYearViewModel = new ChooseYearViewModel(UserModel);
            ChooseYearViewModel.ChooseYearEvent += (_,__) => OnChoosePlannerYear(__);
        }
        public int CurrentView 
        {
            get => _currentView;
            set => RaiseAndSetIfChanged(ref _currentView, value);
        }
        public ChooseYearViewModel ChooseYearViewModel { get; set; }
        public PlannerYearViewModel PlannerYearViewModel
        {
            get => _plannerYearViewModel;
            set => RaiseAndSetIfChanged(ref _plannerYearViewModel, value);
        }
        public UserModel UserModel;
        private void OnChoosePlannerYear(object parameter)
        {
            var yearString = (string)parameter;
            PlannerYearViewModel = new PlannerYearViewModel(UserModel, yearString);
            PlannerYearViewModel.SwitchViewEvent += (_, __) => SwitchView();
            SwitchView();
        }
        public void SwitchView()
        {
            CurrentView = CurrentView == 1 ? 2 : 1;
        }
        
    }
}
