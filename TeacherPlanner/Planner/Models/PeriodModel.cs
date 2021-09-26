using Database;
using Database.DatabaseModels;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.Models
{
    public class PeriodModel : ObservableObject
    {
        private readonly string _delimiter = "`";
        private string _marginText;
        private string _mainText;
        private string _sideText;

        private int? _id = null;
        private int _dayID;
        private int? _timetableClasscodeID;
        private string _userEnteredClasscode;
        private string _classcode;

        public PeriodModel(Period period)
        {
            ID = period.ID;
            _dayID = period.DayID;
            _timetableClasscodeID = period.TimetableClasscode;

            Number = period.PeriodNumber;
            Classcode = ChooseCorrectClasscode(period.UserEnteredClasscode);
            
            MarginText = period.MarginText;
            MainText = period.MainText;
            SideText = period.SideText;
        }

        public int? ID 
        {
            get => _id;
            set
            {
                if (_id == null)
                    _id = value;
            }
        }

        public string MarginText
        {
            get => _marginText;
            set => RaiseAndSetIfChanged(ref _marginText, value);
        }

        public string MainText
        {
            get => _mainText;
            set => RaiseAndSetIfChanged(ref _mainText, value);
        }

        public string SideText
        {
            get => _sideText;
            set => RaiseAndSetIfChanged(ref _sideText, value);
        }

        public int Number { get; set; }
        public string Classcode 
        {
            get => _classcode;
            set
            {
                if (RaiseAndSetIfChanged(ref _classcode, value))
                    _userEnteredClasscode = value;
            } 
        }


        public Period GetDBModel()
        {
            return new Period()
            {
                ID = (int)ID,
                DayID = _dayID,
                TimetableClasscode = _timetableClasscodeID,
                PeriodNumber = Number,
                UserEnteredClasscode = _userEnteredClasscode,
                MarginText = MarginText,
                MainText = MainText,
                SideText = SideText,
            };
        }

        private string ChooseCorrectClasscode(string userEnteredClasscode)
        {
            if (userEnteredClasscode != null && userEnteredClasscode != string.Empty)
            {
                _userEnteredClasscode = userEnteredClasscode;
                return userEnteredClasscode;
            }
            else if (_timetableClasscodeID != null)
            {
                var timetableClasscode = DatabaseManager.GetTimetablePeriodClasscode((int)_timetableClasscodeID);
                if (timetableClasscode != null)
                    return timetableClasscode;
            }
            
            return string.Empty;
        }
    }
}
