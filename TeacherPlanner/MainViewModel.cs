using System.Windows.Input;
using TeacherPlanner.AcademicYearConfiguration.ViewModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.ViewModels;
using TeacherPlanner.PlannerYear.Models;
using TeacherPlanner.PlannerYear.ViewModels;

namespace TeacherPlanner
{
    public class MainViewModel : ObservableObject
    {
        private MainViewState _currentView;
        PlannerYearViewModel _plannerYearViewModel;
        public ICommand ChoosePlannerYearCommand { get; }
        public ICommand SwitchViewCommand { get; }
        public MainViewModel(UserModel userModel)
        {
            UserModel = userModel;
            CurrentView = MainViewState.ChooseYearPage;

            ChooseYearViewModel = new ChooseYearViewModel(UserModel);
            AcademicYearConfigurationViewModel = new AcademicYearConfigurationViewModel();
            AcademicYearConfigurationViewModel.CancelConfigurationEvent += (_, __) => SwitchView(MainViewState.ChooseYearPage);
            AcademicYearConfigurationViewModel.ConfirmConfigurationEvent += (_, __) => OnConfirmAcademicYearConfiguration();

            // Not sure if the line below does anything, so I have commented it out for the time being.
            //ChoosePlannerYearCommand = new SimpleCommand(year => OnChoosePlannerYear((AcademicYearModel)year));
            SwitchViewCommand = new SimpleCommand(view => SwitchView((MainViewState)view));

            ChooseYearViewModel.ChooseYearEvent += (_, __) => OnChoosePlannerYear(__);
        }
        public MainViewState CurrentView
        {
            get => _currentView;
            set => RaiseAndSetIfChanged(ref _currentView, value);
        }
        public AcademicYearConfigurationViewModel AcademicYearConfigurationViewModel { get; set; }
        public ChooseYearViewModel ChooseYearViewModel { get; set; }
        public PlannerYearViewModel PlannerYearViewModel
        {
            get => _plannerYearViewModel;
            set => RaiseAndSetIfChanged(ref _plannerYearViewModel, value);
        }
        public UserModel UserModel { get; }
        private void OnChoosePlannerYear(object parameter)
        {
            var year = (AcademicYearModel)parameter;
            AcademicYearConfigurationViewModel.CurrentAcademicYear = year;
            SwitchView(MainViewState.AcademicYearConfiguration);
        }

        private void OnConfirmAcademicYearConfiguration()
        {
            SwitchView(MainViewState.LoadingPlannerPage);
            var year = AcademicYearConfigurationViewModel.CurrentAcademicYear;
            PlannerYearViewModel = new PlannerYearViewModel(UserModel, year);
            PlannerYearViewModel.SwitchViewToChooseYearViewEvent += (_, __) => SwitchView(__);
            PlannerYearViewModel.SwitchViewToConfigureAcademicYearViewEvent += (_, __) => SwitchView(__);
            SwitchView(MainViewState.PlannerPage);


            // If timetable not imported - prompt the user to import it.
            if (PlannerYearViewModel.TimetableViewModel.TryGetImportedTimetable() == false)
                PlannerYearViewModel.OnImportTimetable();
            // IF timetable weeks not defined - prompt the user to define them.
            if (PlannerYearViewModel.TimetableViewModel.TimetableWeeksAreDefined == false)
                PlannerYearViewModel.OnDefineTimetableWeeks();
        }


        public void SwitchView(object view)
        {
            var newViewState = (MainViewState)view;
            CurrentView = newViewState;

            //TODO update this if any other states are added
            //CurrentView = CurrentView == MainViewState.ChooseYearPage ? MainViewState.PlannerPage : MainViewState.ChooseYearPage;
        }

    }
}
