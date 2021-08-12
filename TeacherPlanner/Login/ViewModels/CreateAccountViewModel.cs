using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login.ViewModels
{
    public class CreateAccountViewModel : ObservableObject
    {
        private string _username;
        private string _feedbackForCreateUsername;
        private string _feedbackForCreatePassword;
        private readonly string _path;
        private readonly string _filename;
        private readonly string _secret = "password12345678password12345678";
        public CreateAccountViewModel(string path, string filename)
        {
            CreateAccountButtonClickedCommand = new SimpleCommand(password => OnCreateAccountButtonClicked(password));
            OnPasswordChangeCommand = new SimpleCommand(password => OnPasswordChange(password));
            _path = path;
            _filename = filename;
            FeedbackForCreateUsername = "";
            FeedbackForCreatePassword = "";
        }
        public string Username 
        { 
            get => _username;
            set 
            {
                _username = value.Trim();
                ParseUsername(_username);
                
            }
        }
        
        private string PasswordHash { get; set; }
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
            
        public bool UsernameIsValidFormat { get; set; } = false;
        public bool PasswordIsValidFormat { get; set; } = false;
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
            if (password.Length > 3)
            {
                FeedbackForCreatePassword = "Password is Valid";
                PasswordIsValidFormat = true;
                PasswordHash = SecurePasswordHasher.Hash(password.Trim());
            }
            else
            {
                FeedbackForCreatePassword = "Password is too short";
                PasswordIsValidFormat = false;
            }
        }
        
        public void OnPasswordChange(object password)
        {
            PasswordBox passwordBox = (PasswordBox)password;
            ParsePassword(passwordBox.Password);
        }
        private void StoreCredentials()
        {
            string key = PasswordHash.Substring(0,32);
            var encryptedUsername = Cryptography.EncryptString(key, Username);
            var encryptedPasswordHash = Cryptography.EncryptString(key, PasswordHash);
            FileHandlingHelper.AppendTo(_path, _filename, $"{encryptedUsername} {encryptedPasswordHash}");
            var user = Cryptography.EncryptString(_secret, Username);
            FileHandlingHelper.AppendTo(_path, "users.txt", user);
        }

        private string[] GetAccountHashes()
        {
            var path = Path.Combine(_path, _filename);
            if (File.Exists(path))
                return File.ReadAllLines(path);
            else
                return new string[0];
        }

        private bool CheckUserExists(string username)
        {
            var path = Path.Combine(_path, "users.txt");
            if (!File.Exists(path))
            {
                return false;
            }
            var users = File.ReadAllLines(path);
            for (int i = 0; i < users.Length; i++)
            {
                if (username == Cryptography.DecryptString(_secret, users[i].Trim()))
                {
                    return true;
                }
            }
            return false;
        }

        public void OnCreateAccountButtonClicked(object password)
        {       
            if (UsernameIsValidFormat && PasswordIsValidFormat)
            {
                if (!CheckUserExists(Username))
                {
                    StoreCredentials();
                    MessageBox.Show("Account Created Successfully");
                }
                else
                {
                    MessageBox.Show("User already exists");
                }
            }
            else
            {
                MessageBox.Show($"Username:{UsernameIsValidFormat}, Password:{PasswordIsValidFormat}");
            }
        }

    }
}
