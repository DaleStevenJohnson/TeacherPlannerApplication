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
        private string _key;

        public UserModel()
        {
            Username = "";
        }
        public string Username
        {
            get => _username;
            set => _username = value;
        }
        public string Key
        { 
            get => _key;
            set => _key = value.Length > 32 ? value.Substring(0, 32) : value;
        }
    }
}
