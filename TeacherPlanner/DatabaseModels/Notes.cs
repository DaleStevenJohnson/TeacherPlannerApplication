using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.DatabaseModels
{
    class Notes
    {
        // PK
        public int ID { get; set; }
        // FK1
        public int DayID { get; set; }

        // Data
#nullable enable
        public string? Text { get; set; }
#nullable disable
    }
}
