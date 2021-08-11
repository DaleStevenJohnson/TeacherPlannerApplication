using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login
{
    class LoginViewModel
    {
        public LoginViewModel()
        {
            LoginDataModel = new LoginModel();
            LoginButtonClickedCommand = new SimpleCommand(password => OnLoginButtonClicked(password));
            OnUsernameChangeCommand = new SimpleCommand(_ => OnUsernameChange());
            OnPasswordChangeCommand = new SimpleCommand(password => OnPasswordChange(password));
        }
        private LoginModel LoginDataModel { get; set; }
        public string Username { get; set; } = "";
        public string FeedbackForCreateUsername { get; set; } = "";
        public string FeedbackForCreatePassword { get; set; } = "";
        public bool UsernameIsValidFormat { get; set; } = false;
        public bool PasswordIsValidFormat { get; set; } = false;
        public UserControl CurrentPage { get; set; }
        public ICommand LoginButtonClickedCommand { get; }
        public ICommand OnUsernameChangeCommand { get; }
        public ICommand OnPasswordChangeCommand { get; }
        public void OnUsernameChange()
        {
            UsernameIsValidFormat = LoginDataModel.ParseUsername(Username, out string feedback);
            FeedbackForCreateUsername = feedback;
        }

        public void OnPasswordChange(object password)
        {
            PasswordBox passwordBox = (PasswordBox)password;
            PasswordIsValidFormat = LoginDataModel.ParsePassword(passwordBox.Password, out string feedback);
            FeedbackForCreatePassword = feedback;
        }

        public void OnLoginButtonClicked(object password)
        {
            var passwordBox = (PasswordBox)password;

            LoginDataModel.TryLogin(Username, passwordBox.Password);
            if (LoginDataModel.LoggedIn)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Invalid Credentials");
            }
        }
    }
}
