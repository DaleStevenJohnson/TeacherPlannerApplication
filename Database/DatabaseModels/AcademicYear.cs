﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Database.DatabaseModels
{
    public class AcademicYear
    {
        // PK
        public int ID { get; set; }
        
        // FK1
        public int UserID { get; set; }
        
        // Data
        public int Year { get; set; }
    }
}
