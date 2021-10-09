using System;

namespace TeacherPlanner.Planner.ViewModels
{
    public class KeyDateItemViewModel
    {
        public KeyDateItemViewModel(int id, string description, string keydatetype, DateTime date)
        {
            ID = id;
            Description = description;
            Type = keydatetype;
            Date = date;
            IsChecked = false;
            DaysUntil = CalculateDaysUntil();
        }

        // Properties
        public int ID { get; }
        public int DaysUntil { get; }
        public bool IsChecked { get; set; }
        public string Description { get; }
        public string Type { get; }
        public DateTime Date { get; }

        // Methods

        public string GetProperty(string property)
        {
            return property.ToLower() switch
            {
                "description" => Description,
                "date" => Date.ToString("yyyy/MM/dd"),
                "type" => Type,
                "time" => Date.ToString("HH:mm"),
                "days until" => DaysUntil.ToString(),
                _ => "Not Found",
            };
        }

        public override string ToString()
        {
            return $"{Description} - {Type} - {Date:HH:mm}";
        }

        private int CalculateDaysUntil()
        {
            if (Date > DateTime.Today)
                return (Date - DateTime.Today).Days;
            return 0;
        }
    }
}
