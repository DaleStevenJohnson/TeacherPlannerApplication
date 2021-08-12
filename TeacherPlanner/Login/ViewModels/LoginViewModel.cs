using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;

namespace TeacherPlanner.Login.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private readonly string _path;
        private readonly string _filename;
        private string _username;

        public LoginViewModel(string accountFile, string filename)
        {
            UserModel = new UserModel();
            LoginButtonClickedCommand = new SimpleCommand(password => OnLoginButtonClicked(password));
            _path = accountFile;
            _filename = filename;
            LoggedIn = false;
        }
        public bool LoggedIn { get; set; }
        public string Username 
        {
            get => UserModel.Username;
            set
            {
                RaiseAndSetIfChanged(ref _username, value.Trim());
                UserModel.Username = _username;
            }
        }
        public UserModel UserModel { get; set; }
        public UserControl CurrentPage { get; set; }
        public ICommand LoginButtonClickedCommand { get; }
        
        private void OnLoginButtonClicked(object parameter)
        {
            var values = (object[])parameter;
            var window = (Window)values[0];
            var passwordBox = (PasswordBox)values[1];

            LoggedIn = Authenticate(Username, passwordBox.Password.Trim());
            if (LoggedIn)
            {
                UserModel.Key = Cryptography.EncryptString(
                    string.Concat(Enumerable.Repeat(passwordBox.Password.Trim().Substring(0, 4), 8)),
                    string.Concat(Enumerable.Repeat(passwordBox.Password.Trim().Substring(4, 4), 8))
                    );
                //MessageBox.Show("Success");
                window.Close();
            }
            else
            {
                MessageBox.Show("Invalid Credentials");
            }
        }

        private string[] GetAccountHashes()
        {
            var path = Path.Combine(_path, _filename);
            if (File.Exists(path))
                return File.ReadAllLines(path);
            else
                return new string[0];
        }
        private bool Authenticate(string username, string password)
        {
            var data = GetAccountHashes();
            for (int i = 0; i < data.Length; i++)
            {
                if (SecurePasswordHasher.Verify(username + password, data[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
