using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Database;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;

namespace TeacherPlanner.Login.ViewModels
{
    public class CreateAccountViewModel : ObservableObject
    {
        private string _username;
        private string _feedbackForCreateUsername;
        private string _feedbackForCreatePassword;
        private bool _validCredentials;
        private bool _usernameIsValidFormat;
        private bool _passwordIsValidFormat;

        public CreateAccountViewModel()
        {
            CreateAccountButtonClickedCommand = new SimpleCommand(password => OnCreateAccountButtonClicked());
            OnPasswordChangeCommand = new SimpleCommand(password => OnPasswordChange(password));
            FeedbackForCreateUsername = string.Empty;
            FeedbackForCreatePassword = string.Empty;
            UsernameIsValidFormat = false;
        }
        public string Username 
        { 
            get => _username;
            set 
            {
                if (RaiseAndSetIfChanged(ref _username, value.Trim().ToLower()))
                    ParseUsername(_username);
            }
        }
        
        private string PasswordHash { get; set; }
        public bool UsernameIsValidFormat 
        {
            get => _usernameIsValidFormat;
            set
            {
                _usernameIsValidFormat = value;
                ValidCredentials = value && PasswordIsValidFormat;
            }
        }
        public bool PasswordIsValidFormat
        {
            get => _passwordIsValidFormat;
            set
            {
                _passwordIsValidFormat = value;
                ValidCredentials = value && UsernameIsValidFormat;
            }
        }



        public bool ValidCredentials
        {
            get => _validCredentials;
            set => RaiseAndSetIfChanged(ref _validCredentials, value);
        }

        public ICommand CreateAccountButtonClickedCommand { get; }
        public ICommand OnPasswordChangeCommand { get; }
        public string FeedbackForCreateUsername 
        {
            get => _feedbackForCreateUsername;
            set => RaiseAndSetIfChanged(ref _feedbackForCreateUsername, value);
        }
        public string FeedbackForCreatePassword
        {
            get => _feedbackForCreatePassword;
            set => RaiseAndSetIfChanged(ref _feedbackForCreatePassword, value);
        }
            
        
        public void ParseUsername(string username)
        {
            if (username.Length > 3)
            {
                FeedbackForCreateUsername = "Username is Valid";
                UsernameIsValidFormat = true;
            }
            else
            {
                FeedbackForCreateUsername = "Username is too short";
                UsernameIsValidFormat = false;
            }
        }

        public void ParsePassword(string password)
        {
            var minimumSize = 8;
            if (password.Length >= minimumSize)
            {
                FeedbackForCreatePassword = "Password is Valid";
                PasswordIsValidFormat = true;
                PasswordHash = SecurePasswordHasher.Hash(password.Trim());
            }
            else
            {
                FeedbackForCreatePassword = $"Password is too short - must be at least {minimumSize} characters long";
                PasswordIsValidFormat = false;
            }
        }
        
        public void OnPasswordChange(object password)
        {
            PasswordBox passwordBox = (PasswordBox)password;
            ParsePassword(passwordBox.Password);
        }
        private bool StoreCredentials()
        {
            // SQLite DB implementation           
            var user = new User()
            {
                Username = Username,
                Password = PasswordHash
            };
            return DatabaseManager.TryAddUser(user);
        }

        public void OnCreateAccountButtonClicked()
        {
            if (!UsernameIsValidFormat || !PasswordIsValidFormat)
            {
                // Todo - replace the below message box with locking of the button on the UI.
                MessageBox.Show($"Username:{UsernameIsValidFormat}, Password:{PasswordIsValidFormat}");
                return;
            }

            if (DatabaseManager.CheckIfUserExists(Username))
            {
                Username = string.Empty;
                MessageBox.Show("User already exists");
                return;
            }
            
            if (StoreCredentials())
                MessageBox.Show("Account Created Successfully");
            else
                MessageBox.Show("Error creating account");
        }
    }
}
