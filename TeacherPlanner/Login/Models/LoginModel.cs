using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login.Models
{
    public class LoginModel
    {
        public LoginModel()
        {
            LoggedIn = false;
        }       
        public bool LoggedIn { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
  
    }
}
