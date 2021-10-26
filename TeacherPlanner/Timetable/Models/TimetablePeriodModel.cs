using System;
using System.Collections.Generic;
using System.Text;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Timetable.Models
{
    public class TimetablePeriodModel : ObservableObject
    {
        private string _classCode;
        private string _room;
        private int _occurance;
        private int _occurances;

        public TimetablePeriodModel(TimetablePeriod timetablePeriod, TimetableDisplayModes displayMode)
        {
            DisplayMode = displayMode;
            if (timetablePeriod != null)
            {
                ID = timetablePeriod.ID;
                ClassCode = timetablePeriod.ClassCode;
                Room = timetablePeriod.RoomCode;
            }
        }
        public int ID { get; }
        public string ClassCode 
        {
            get => _classCode;
            set => RaiseAndSetIfChanged(ref _classCode, value);
        }

        public string Room
        {
            get => _room;
            set => RaiseAndSetIfChanged(ref _room, value);
        }

        public int Occurance
        {
            get => _occurance;
            set => RaiseAndSetIfChanged(ref _occurance, value);
        }

        public int Occurances
        {
            get => _occurances;
            set => RaiseAndSetIfChanged(ref _occurances, value);
        }

        public TimetableDisplayModes DisplayMode { get; set; }
        public void Clear()
        {
            ClassCode = "";
            Room = "";
            Occurance = 0;
            Occurances = 0;
        }
    }
}
