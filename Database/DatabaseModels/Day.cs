using System;
using System.Collections.Generic;
using System.Text;

namespace Database.DatabaseModels
{
    public class Day
    {
        // PK
        public int ID { get; set; }

        // FK1
        public int AcademicYearID { get; set; }

        // Data
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
