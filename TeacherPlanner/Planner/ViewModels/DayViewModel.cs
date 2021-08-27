using System;
using System.IO;
using System.Windows.Input;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.Timetable.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class DayViewModel : ObservableObject
    {
        private DayModel _dayModel;
        private CalendarModel _calendarModel;

        public event EventHandler<AdvancePageState> TurnPageEvent;
        public ICommand TurnPageCommand { get; }
        public DayViewModel(UserModel userModel, DateTime date, TimetableModel timetable, string side)
        {
            UserModel = userModel;
            Timetable = timetable;
            DayModel = LoadAndPopulateNewDay(date);
            TurnPageCommand = new SimpleCommand(numOfDays => OnTurnPage(numOfDays));

            if (side == "left")
            {
                Forward1 = AdvancePageState.LeftForward1;
                Forward7 = AdvancePageState.LeftForward7;
                ForwardMonth = AdvancePageState.LeftForwardMonth;
                Backward1 = AdvancePageState.LeftBackward1;
                Backward7 = AdvancePageState.LeftBackward7;
                BackwardMonth = AdvancePageState.LeftBackwardMonth;
            }
            else
            {
                Forward1 = AdvancePageState.RightForward1;
                Forward7 = AdvancePageState.RightForward7;
                ForwardMonth = AdvancePageState.RightForwardMonth;
                Backward1 = AdvancePageState.RightBackward1;
                Backward7 = AdvancePageState.RightBackward7;
                BackwardMonth = AdvancePageState.RightBackwardMonth;
            }
        }
        private TimetableModel Timetable { get; }
        private void OnTurnPage(object v)
        {
            TurnPageEvent.Invoke(null, (AdvancePageState)v);
        }
        public UserModel UserModel { get; }
        public AdvancePageState Forward1 { get; }
        public AdvancePageState Forward7 { get; }
        public AdvancePageState ForwardMonth { get; }
        public AdvancePageState Backward1 { get; }
        public AdvancePageState Backward7 { get; }
        public AdvancePageState BackwardMonth { get; }
        public CalendarModel CalendarModel 
        {
            get => _calendarModel;
            set => RaiseAndSetIfChanged(ref _calendarModel, value);
        }
        public DayModel DayModel
        {
            get => _dayModel;
            set => RaiseAndSetIfChanged(ref _dayModel, value);
        }

        public DayModel LoadAndPopulateNewDay(DateTime date, bool overwriteClassCode = false)
        {
            var filenameDate = date.ToString(Formats.FullDateFormat);
            // Create path for where data should be stored for the provided date
            var filename = FileHandlingHelper.EncryptFileOrDirectory(filenameDate + ".txt");
            var path = Path.Combine(FileHandlingHelper.CreateMonthlyUserDataDirectory(filenameDate, UserModel.Key), filename);

            // Read data from file. If file does not exist, string[] data will be an empty array
            var data = FileHandlingHelper.ReadDataFromFile(path, true, UserModel.Key);

            // The file is saved with six periods and a notes section one after the other.
            // The numbers below define sections of that file within the loop in order to select the correct part of the file
            var periodRows = 7;
            var jump = 8;
            var position = 0;

            // Update Calendar Model to a new Instance with the new date
            CalendarModel = new CalendarModel(date);

            DayModel newDayModel = new DayModel(date);
            // If this is false, it means no save file exists, and we need to just go ahead and create a completely empty DayModel
            if (data.Length > 0 && Timetable != null)
            {
                // Repeat for all six periods
                for (var i = 0; i < newDayModel.Periods.Length; i++)
                {
                    // Initialise empty array to store all data for this current period
                    var periodData = new string[periodRows];

                    // This is the index in the string[] data array where the class code is located
                    var classCodeIndex = i * jump;

                    // This is the index in the string[] data array where the periodData begins
                    var startIndex = classCodeIndex + 1;

                    // This keeps track of where we are in the periodData array,
                    // as the loop below is going through the string[] data array
                    var periodIndex = 0;

                    // This loop populates the periodData array with all data for the current period
                    for (var j = startIndex; j < startIndex + periodRows; j++)
                    {
                        periodData[periodIndex] = data[j];
                        periodIndex++;
                    }

                    var classCode = overwriteClassCode ? string.Empty : data[classCodeIndex];

                    if (classCode == string.Empty)
                    {
                        var day = (int)date.DayOfWeek;
                        var week = CalendarManager.GetWeek(date);
                        if (week == 1 || week == 2)
                        {
                            var timetablePeriodModel = Timetable.GetPeriod(week, day, (i + 1).ToString());
                            classCode = timetablePeriodModel.ClassCode;
                        }
                        else if (week == 3)
                            classCode = "Holiday";
                    }
                    // The below method call gets the day model to create a new period using the data supplied
                    newDayModel.LoadPeriodDataIntoNewPeriod(i + 1, classCode, periodData);

                    // This keeps track of what line we are on in the data array
                    // It will be needed once we finish adding periods, but still have
                    // data to add from the string[] data array
                    position = classCodeIndex + periodRows;
                }

                // Last in the file is the note data
                // Establishes the expected size of the data
                var noteRows = 6;
                var noteData = new string[noteRows];
                var noteIndex = 0;

                // Loops through and compiles the note data into string[]
                for (var j = position + 1; j < position + 1 + noteRows; j++)
                {
                    noteData[noteIndex] = data[j];
                    noteIndex++;
                }

                // data is added into the NoteSection Model attached to the new DayModel that will be returned from this method
                newDayModel.NoteSectionModel.Load(noteData);
            }
            else
            {
                newDayModel.LoadEmptyPeriods();
                newDayModel.LoadEmptyIntoNewNoteSection();
            }
            return newDayModel;
        }
        internal void SaveDayToFile()
        {
            var filenameDate = DayModel.Date.ToString(Formats.FullDateFormat);
            var saveData = DayModel.PackageSaveData();
            var filename = FileHandlingHelper.EncryptFileOrDirectory(filenameDate + ".txt", UserModel.Key);

            // Final save path looks like this, for user Bob on 15th January 1970: \Bob\1970\197001\19700115.txt
            var path = FileHandlingHelper.CreateMonthlyUserDataDirectory(filenameDate, UserModel.Key);
            FileHandlingHelper.TryWriteDataToFile(path, filename, saveData, "o", true, UserModel.Key);
        }
    }
}
