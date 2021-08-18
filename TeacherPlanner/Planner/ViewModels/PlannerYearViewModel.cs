using System;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Timetable.ViewModels;
using TeacherPlanner.ToDo.ViewModels;

namespace TeacherPlanner.Planner.ViewModels
{
    public class PlannerYearViewModel : ObservableObject
    {
        private TimetableViewModel _timetableViewModel;

        public ICommand DefineTimetableWeeksCommand { get; }
        public event EventHandler<string> SwitchViewEvent;
        public ICommand SwitchViewCommand { get; }
        public PlannerYearViewModel(UserModel userModel, string yearString)
        {
            UserModel = userModel;
            FileHandlingHelper.SetDirectories(UserModel, yearString);

            CalendarManager = new CalendarManager(yearString);

            TimetableViewModel = new TimetableViewModel(UserModel);
            PlannerViewModel = new PlannerViewModel(UserModel, TimetableViewModel.CurrentTimetable, CalendarManager);
            ToDoViewModel = new ToDoViewModel(UserModel);
            
            DefineTimetableWeeksCommand = new SimpleCommand(_ => OnDefineTimetableWeeks());
            SwitchViewCommand = new SimpleCommand(_ => OnSwitchView(_));
            
        }

        public CalendarManager CalendarManager { get; private set; }
        public UserModel UserModel { get; }
        public PlannerViewModel PlannerViewModel { get; }
        public ToDoViewModel ToDoViewModel { get; }
        public TimetableViewModel TimetableViewModel 
        {
            get => _timetableViewModel;
            set => RaiseAndSetIfChanged(ref _timetableViewModel, value);
        }
        private void OnDefineTimetableWeeks()
        {
            if (TimetableViewModel.DefineTimetableWeeks(UserModel) ?? true)
                PlannerViewModel.LoadNewDays(true);
        }
        private void OnSwitchView(object v)
        {
            SwitchViewEvent.Invoke(null, (string)v);
        }
    }
}
