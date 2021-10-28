using System;
using System.Collections.ObjectModel;
using System.Linq;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.Models
{
    public class DayModel : ObservableObject
    {
        private string _notes;
        private ObservableCollection<PeriodModel> _periods;
        
        private int _academicYearID;
        private int? _id = null;
        private ObservableCollection<PeriodModel> _lessons;

        public DayModel(Day day, ObservableCollection<PeriodModel> periods)
        {
            ID = day.ID;
            _academicYearID = day.AcademicYearID;

            Date = day.Date;
            Notes = day.Notes;
            Periods = periods;
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

        public ObservableCollection<PeriodModel> Lessons
        {
            get => _lessons;
            set => RaiseAndSetIfChanged(ref _lessons, value);
        }

        public ObservableCollection<PeriodModel> Periods 
        {
            get => _periods;
            set
            {
                if (RaiseAndSetIfChanged(ref _periods, value))
                    Lessons = new ObservableCollection<PeriodModel>(value.Where(p => p.Number < PeriodCodes.Break1));
            }
        }

        public string Notes 
        { 
            get => _notes;
            set
            {
                if (TryParseNotes(value))
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

        private bool TryParseNotes(string notes)
        {
            if (notes == null)
                return false;

            int MAX_CHARS = 72 * 6;
            if (notes.Length > MAX_CHARS)
                return false;

            int linebreaks = 0;
            for (int i = 0; i < notes.Length-4; i++)
            {
                if (notes.Substring(i, 4) == Environment.NewLine)
                    linebreaks += 1;
            }
            
            return linebreaks < 6;
        }
    }
}
