using System;
using System.Collections.Generic;
using System.Text;

namespace Database.DatabaseModels
{
    class TodoList
    {
        // PK
        public int ID { get; set; }
        // FK1
        public int AcademicYearID { get; set; }
        
        // Data
        public string Name { get; set; }
    }
}
