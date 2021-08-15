using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.ViewModels;
using TeacherPlanner.Timetable.ViewModels;
using TeacherPlanner.ToDo.ViewModels;

namespace TeacherPlanner
{
    public class MainViewModel
    {
        public MainViewModel(UserModel userModel)
        {
            UserModel = userModel;
            TimetableViewModel = new TimetableViewModel(UserModel);
            PlannerViewModel = new PlannerViewModel(UserModel, TimetableViewModel.CurrentTimetable);
            ToDoViewModel = new ToDoViewModel(UserModel);
            
        }
        public UserModel UserModel;
        public PlannerViewModel PlannerViewModel { get; }
        public ToDoViewModel ToDoViewModel { get; }
        public TimetableViewModel TimetableViewModel { get;  }
    }
}
