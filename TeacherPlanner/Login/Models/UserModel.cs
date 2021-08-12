using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login.Models
{
    public class UserModel
    {
        private string _username;
        public UserModel()
        {
            Username = "";
        }
        public string Username
        {
            get => _username;
            set => Username = value;
        }
        public string PasswordHash { get; set; }
    }
}
