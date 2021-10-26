using System.IO;
using System.Windows.Controls;
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

        public void OnTabChanged(object sender, SelectionChangedEventArgs e)
        {
            LoginViewModel.Username = "";
            CreateAccountViewModel.Username = "";
        }
    }
}
