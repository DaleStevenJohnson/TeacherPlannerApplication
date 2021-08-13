using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.ViewModels;
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
        }
        public UserModel UserModel;
        public PlannerViewModel PlannerViewModel { get; }
        public ToDoViewModel ToDoViewModel { get; }
    }
}
