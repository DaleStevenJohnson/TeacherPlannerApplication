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
    public class MainViewModel
    {
        public ICommand ChoosePlannerYearCommand { get; }
        public MainViewModel(UserModel userModel)
        {
            UserModel = userModel;
            ChoosePlannerYearCommand = new SimpleCommand(yearString => OnChoosePlannerYear((string)yearString));
        }
        public PlannerYearViewModel PlannerYearViewModel { get; set; }
        public List<YearSelectModel> YearSelectModels { get; set; }
        public UserModel UserModel;
        private void OnChoosePlannerYear(string yearString)
        {
            PlannerYearViewModel = new PlannerYearViewModel(UserModel, yearString);
        }
    }
}
