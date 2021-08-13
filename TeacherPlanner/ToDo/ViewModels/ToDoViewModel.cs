using TeacherPlanner.Login.Models;

namespace TeacherPlanner.ToDo.ViewModels
{
    public class ToDoViewModel
    {
        public ToDoViewModel(UserModel userModel)
        {
            UserModel = userModel;
        }
        public UserModel UserModel;
    }
}
