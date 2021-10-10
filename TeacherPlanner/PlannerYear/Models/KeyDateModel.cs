using System;

namespace TeacherPlanner.PlannerYear.Models
{
    public class KeyDateModel
    {
        public KeyDateModel(string description, string datetype, DateTime datetime)
        {
            Description = description;
            Type = datetype;
            DateTime = datetime;
        }

        public string Description { get; }
        public string Type { get; }
        public DateTime DateTime { get; }
    }
}
