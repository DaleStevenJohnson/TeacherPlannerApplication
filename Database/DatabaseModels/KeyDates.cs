using System;
using System.Collections.Generic;
using System.Text;

namespace Database.DatabaseModels
{
    public class KeyDates
    {
        // PK
        public int ID { get; set; }
        // FK1
        public int AcademicYear { get; set; }

        // Data
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
    }
}
