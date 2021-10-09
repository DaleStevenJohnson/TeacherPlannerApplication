using Database;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
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
        private string _classcode;
        private string _userEnteredClasscode;

        public PeriodModel(Period period)
        {
            ID = period.ID;
            _dayID = period.DayID;
            _timetableClasscodeID = period.TimetableClasscode;

            Number = (PeriodCodes)period.PeriodNumber;
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

        public PeriodCodes Number { get; set; }
        public string Classcode 
        {
            get => _classcode;
            set => RaiseAndSetIfChanged(ref _classcode, value);
        }

        private string UserEnteredClasscode
        {
            get => _userEnteredClasscode;
            set
            {
                if (RaiseAndSetIfChanged(ref _userEnteredClasscode, value))
                {
                    if (value == string.Empty)
                        Classcode = GetTimetablePeriodClasscode();
                }
            }
        }

        public void UpdateUserEnteredClassCode(string userinput)
        {
            UserEnteredClasscode = userinput;
        }

        public Period GetDBModel()
        {
            return new Period()
            {
                ID = (int)ID,
                DayID = _dayID,
                TimetableClasscode = _timetableClasscodeID,
                PeriodNumber = (int)Number,
                UserEnteredClasscode = UserEnteredClasscode,
                MarginText = MarginText,
                MainText = MainText,
                SideText = SideText,
            };
        }

        private string ChooseCorrectClasscode(string userEnteredClasscode)
        {
            if (userEnteredClasscode != null && userEnteredClasscode != string.Empty)
            {
                UserEnteredClasscode = userEnteredClasscode;
                return userEnteredClasscode;
            }
            else if (_timetableClasscodeID != null)
            {
                return GetTimetablePeriodClasscode();
            }
            
            return string.Empty;
        }

        private string GetTimetablePeriodClasscode()
        {
            var timetableClasscode = DatabaseManager.GetTimetablePeriodClasscode((int)_timetableClasscodeID);
            if (timetableClasscode != null)
                return timetableClasscode;
            return string.Empty;
        }

        
    }
}
