using System;
using System.Collections.Generic;
using System.Text;

namespace Database.DatabaseModels
{
    class Period
    {
#nullable enable
        //PK
        public int ID { get; set; }
        // FK1
        public int DayID { get; set; }
        // FK2
        public int? TimetableClasscode { get; set; }
        
        // Data
        public int PeriodNumber { get; set; }
        public string? UserEnteredClasscode { get; set; }
        public string? MarginText { get; set; }
        public string? MainText { get; set; }
        public string? SideText { get; set; }
#nullable disable
    }
}
