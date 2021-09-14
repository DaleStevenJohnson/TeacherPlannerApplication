using System;
using System.Collections.Generic;
using System.Text;

namespace Database.DatabaseModels
{
    class TimetablePeriod
    {
        // PK
        public int ID { get; set; }
        // FK1
        public int AcademicYearID { get; set; }

        // Data
        public int Week { get; set; }
        public int Day { get; set; }
        public int Period { get; set; }
        public string ClassCode { get; set; }
        public string RoomCode { get; set; }
    }
}
