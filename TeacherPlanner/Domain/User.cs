using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Domain
{
    public class User
    {
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }

}
