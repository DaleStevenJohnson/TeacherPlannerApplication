using System.Collections.Generic;
using System.Windows.Input;
using TeacherPlanner.Constants;
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
        private MainViewState _currentView;
        PlannerYearViewModel _plannerYearViewModel;
        public ICommand ChoosePlannerYearCommand { get; }
        public ICommand SwitchViewCommand { get;  }
        public MainViewModel(UserModel userModel)
        {
            UserModel = userModel;
            CurrentView = MainViewState.ChooseYearPage;

            ChooseYearViewModel = new ChooseYearViewModel(UserModel);

            ChoosePlannerYearCommand = new SimpleCommand(year => OnChoosePlannerYear((YearSelectModel)year));
            SwitchViewCommand = new SimpleCommand(_ => SwitchView());
            
            ChooseYearViewModel.ChooseYearEvent += (_,__) => OnChoosePlannerYear(__);
        }
        public MainViewState CurrentView 
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
        public UserModel UserModel { get; }
        private void OnChoosePlannerYear(object parameter)
        {
            var year = (YearSelectModel)parameter;
            PlannerYearViewModel = new PlannerYearViewModel(UserModel, year);
            PlannerYearViewModel.SwitchViewEvent += (_, __) => SwitchView();
            SwitchView();

            if (PlannerYearViewModel.TimetableViewModel.TryGetImportedTimetable() == false)
                PlannerYearViewModel.TimetableViewModel.OnTimetableImportClick();
            
            if (PlannerYearViewModel.TimetableViewModel.TimetableWeeksAreDefined == false)
                PlannerYearViewModel.OnDefineTimetableWeeks();
        }


        public void SwitchView()
        {
            //TODO update this if any other states are added
            CurrentView = CurrentView == MainViewState.ChooseYearPage ? MainViewState.PlannerPage : MainViewState.ChooseYearPage;
        }
        
    }
}
