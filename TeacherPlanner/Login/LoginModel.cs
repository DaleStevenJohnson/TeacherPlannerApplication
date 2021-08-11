using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login
{
    public class LoginModel
    {
        public LoginModel()
        {
            LoggedIn = false;   
        }
        private string HashFile = Path.Combine(FileHandlingHelper.RootPath, "Config", "Hashes.txt");
        public bool LoggedIn { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool UsernameIsValid { get; set; }
        public bool TryLogin(string username, string password)
        {
            if (VerifyUsername(username))
                if (VerifyPassword(password))
                    LoggedIn = true;
                else
                    LoggedIn = false;
            else
                LoggedIn = false;
            return LoggedIn;
        }
     
        public bool ParseUsername(string username, out string usernameFeedback) 
        {
            if (username.Length > 3)
            {
                usernameFeedback = "Username is Valid"; 
                return true;
            }
            else
            {
                usernameFeedback = "Username is too short";
                return false;
            }
            
        }

        public string[] GetPasswordHashes() 
        {
            if (File.Exists(HashFile))
                return File.ReadAllLines(HashFile);
            else
                return new string[0];   
        }

        public bool VerifyUsername(string username)
        {
            string[] storedUsers = new string[] { "bob", "DJohnson", "craig" };
            for (int i = 0; i < storedUsers.Length; i++)
            {
                if (storedUsers[i] == username)
                {
                    UsernameIsValid = true;
                    Username = username;
                    return UsernameIsValid;
                }
            }
            UsernameIsValid = false;
            username = "";
            return UsernameIsValid;
            
        }

        public bool VerifyPassword(string password)
        {
            string[] hashes = GetPasswordHashes();
            for (int i = 0; i < hashes.Length; i++)
            {
                if (SecurePasswordHasher.Verify(password, hashes[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ParsePassword(string password, out string passwordFeedback)
        {
            if (password.Length > 3)
            {
                passwordFeedback = "Password is Valid";
                return true;
            }
            else
            {
                passwordFeedback = "Password is too short";
                return false;
            }
        }
    }
}
