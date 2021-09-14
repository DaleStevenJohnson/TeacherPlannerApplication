using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.DatabaseModels
{
    class UserSettings
    {
        // PK
        public int ID { get; set; }
        
        // FK1
        public int UserID { get; set; }
    }
}
