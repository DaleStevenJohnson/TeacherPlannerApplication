using System.IO;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login.ViewModels
{
    public class AccountManagementViewModel
    {
        public AccountManagementViewModel()
        {
            LoginViewModel = new LoginViewModel();
            CreateAccountViewModel = new CreateAccountViewModel();
        }
        
        public LoginViewModel LoginViewModel { get; set; }
        public CreateAccountViewModel CreateAccountViewModel { get; set; }
    }
}
