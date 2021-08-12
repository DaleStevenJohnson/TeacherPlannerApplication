using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login.ViewModels
{
    public class AccountManagementViewModel
    {
        private readonly string _path = Path.Combine(FileHandlingHelper.RootPath, "Config");
        private readonly string _filename = "Accounts.txt";
        public AccountManagementViewModel()
        {
            LoginViewModel = new LoginViewModel(_path, _filename);
            CreateAccountViewModel = new CreateAccountViewModel(_path, _filename);
        }
        

        public LoginViewModel LoginViewModel { get; set; }
        public CreateAccountViewModel CreateAccountViewModel { get; set; }
}
}
