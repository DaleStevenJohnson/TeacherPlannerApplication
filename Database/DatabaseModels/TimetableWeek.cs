using System;
using System.Collections.Generic;
using System.Text;

namespace Database.DatabaseModels
{
    class TimetableWeek
    {
        // PK
        public int ID { get; set; }
        // FK1
        public int AcademicYearID { get; set; }

        // Data
        public DateTime WeekBeginning { get; set; }
        public int? Week { get; set; }
    }
}
