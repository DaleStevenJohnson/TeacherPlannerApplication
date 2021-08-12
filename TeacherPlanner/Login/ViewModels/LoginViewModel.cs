using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;

namespace TeacherPlanner.Login.ViewModels
{
    public class LoginViewModel
    {
        private readonly string _path;
        private readonly string _filename;
        private string _username;
        public LoginViewModel(string accountFile, string filename)
        {
            LoginModel = new LoginModel();
            LoginButtonClickedCommand = new SimpleCommand(password => OnLoginButtonClicked(password));
            _path = accountFile;
            _filename = filename;
            Username = "";
        }
        private LoginModel LoginModel { get; set; }

        public string Username {
            get => _username;
            set => _username = value.Trim();
        }
        private bool UsernameIsValid { get; set; }
        public UserControl CurrentPage { get; set; }
        public ICommand LoginButtonClickedCommand { get; }
        
        private void OnLoginButtonClicked(object password)
        {
            var passwordBox = (PasswordBox)password;

            LoginModel.LoggedIn = Authenticate(Username, passwordBox.Password.Trim());
            if (LoginModel.LoggedIn)
            {
                MessageBox.Show("Success");
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
            var passwordHash = SecurePasswordHasher.Hash(password);
            password = "";
            string key = passwordHash.Substring(0, 32);
            var data = GetAccountHashes();
            for (int i = 0; i < data.Length; i++)
            {
                var item = data[i].Split(" ");
                var item_username = Cryptography.DecryptString(key, item[0].Trim());
                var item_passwordHash = Cryptography.DecryptString(key, item[1].Trim());
                if (item_username == username && item_passwordHash == passwordHash)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
