﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.DatabaseModels
{
    public class User
    {
        // PK
        public int ID { get; set; }

        // Data
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
