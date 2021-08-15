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
            PlannerViewModel = new PlannerViewModel(UserModel);
            ToDoViewModel = new ToDoViewModel(UserModel);
            TimetableViewModel = new TimetableViewModel(UserModel);
        }
        public UserModel UserModel;
        public PlannerViewModel PlannerViewModel { get; }
        public ToDoViewModel ToDoViewModel { get; }
        public TimetableViewModel TimetableViewModel { get;  }
    }
}
