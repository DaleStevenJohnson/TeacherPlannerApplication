using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.DatabaseModels
{
    class TodoItem
    {
        // PK
        public int ID { get; set; }
        // FK1
        public int TodoListID { get; set; }
        // FK2
        public int? ParentItem { get; set; }

        // Data
        public bool IsSubItem { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
