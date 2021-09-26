using System;
using System.Collections.ObjectModel;
using Database.DatabaseModels;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.Models
{
    public class DayModel : ObservableObject
    {
        private string _notes;
        private ObservableCollection<PeriodModel> _periods;
        
        private int _academicYearID;
        private int? _id = null;

        public DayModel(Day day, ObservableCollection<PeriodModel> periods)
        {
            ID = day.ID;
            _academicYearID = day.AcademicYearID;

            Date = day.Date;
            Notes = day.Notes;
            Periods = Periods;
        }

        // Properties
        public int? ID
        {
            get => _id;
            set
            {
                if (_id == null)
                    _id = value;
            }
        }
        public DateTime Date { get; }
        
        public ObservableCollection<PeriodModel> Periods 
        {
            get => _periods;
            set => RaiseAndSetIfChanged(ref _periods, value);
        }

        public string Notes 
        { 
            get => _notes;
            set
            {
                if (ParseNotes(value))
                    RaiseAndSetIfChanged(ref _notes, value);
            }
        }



        // Public Methods

        public Day GetDBModel()
        {
            return new Day()
            {
                ID = (int)ID,
                AcademicYearID = _academicYearID,
                Date = Date,
                Notes = Notes,
            };
        }



        // Private methods

        private bool ParseNotes(string notes)
        {

            int MAX_CHARS = 72 * 6;
            if (notes.Length > MAX_CHARS)
                return false;

            int linebreaks = 0;
            for (int i = 0; i < notes.Length; i++)
            {
                if (notes.Substring(i, 2) == "\n")
                    linebreaks += 1;
            }
            
            return linebreaks < 6;
        }
    }
}
