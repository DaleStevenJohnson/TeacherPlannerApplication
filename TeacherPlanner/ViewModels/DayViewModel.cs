using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Models;

namespace TeacherPlanner.ViewModels
{
    public class DayViewModel : ObservableObject
    {
        private DayModel _dayModel;

        public event EventHandler<string> TurnPageEvent;
        public ICommand TurnPageCommand { get; }
        public DayViewModel(UserModel userModel, DateTime date)
        {
            UserModel = userModel;
            DayModel = LoadAndPopulateNewDay(date);
            TurnPageCommand = new SimpleCommand(numOfDays => OnTurnPage(numOfDays));
        }

        private void OnTurnPage(object v)
        {
            TurnPageEvent.Invoke(null, (string)v);
        }
        public UserModel UserModel;
        public DayModel DayModel
        { 
            get => _dayModel;
            set => RaiseAndSetIfChanged(ref _dayModel, value);
        }

        private string GetFormattedDateString(DateTime date) => date.ToString("yyyyMMdd");

        public DayModel LoadAndPopulateNewDay(DateTime date)
        {
            var filenameDate = GetFormattedDateString(date);
            // Create path for where data should be stored for the provided date
            string filename = filenameDate + ".txt";
            string path = Path.Combine(FileHandlingHelper.CreateDatedUserDirectory(UserModel.Username, filenameDate), filename);
            
            // Read data from file. If file does not exist, string[] data will be an empty array
            string[] data = FileHandlingHelper.LoadDataFromFile(path, true, UserModel.Key);

            // The file is saved with six periods and a notes section one after the other.
            // The numbers below define sections of that file within the loop in order to select the correct part of the file
            int periodRows = 7;
            int jump = 8;
            int position = 0;


            DayModel newDayModel = new DayModel(date);
            // If this is false, it means no save file exists, and we need to just go ahead and create a completely empty DayModel
            if (data.Length > 0)
            {
                // Repeat for all six periods
                for (int i = 0; i < newDayModel.Periods.Length; i++)
                {
                    // Initialise empty array to store all data for this current period
                    string[] periodData = new string[periodRows];

                    // This is the index in the string[] data array where the class code is located
                    int classCodeIndex = i * jump;

                    // This is the index in the string[] data array where the periodData begins
                    int startIndex = classCodeIndex + 1;

                    // This keeps track of where we are in the periodData array,
                    // as the loop below is going through the string[] data array
                    int periodIndex = 0;

                    // This loop populates the periodData array with all data for the current period
                    for (int j = startIndex; j < startIndex + periodRows; j++)
                    {
                        periodData[periodIndex] = data[j];
                        periodIndex++;
                    }

                    // The below method call gets the day model to create a new period using the data supplied
                    newDayModel.LoadPeriodDataIntoNewPeriod(i + 1, data[classCodeIndex], periodData);

                    // This keeps track of what line we are on in the data array
                    // It will be needed once we finish adding periods, but still have
                    // data to add from the string[] data array
                    position = classCodeIndex + periodRows;
                }

                // Last in the file is the note data
                // Establishes the expected size of the data
                int noteRows = 6;
                string[] noteData = new string[noteRows];
                int noteIndex = 0;
                
                // Loops through and compiles the note data into string[]
                for (int j = position + 1; j < position + 1 + noteRows; j++)
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
            var filenameDate = GetFormattedDateString(DayModel.CalendarModel.Date);
            string saveData = DayModel.PackageSaveData();
            string filename = filenameDate + ".txt";

            // Final save path looks like this, for user Bob on 15th January 1970: \Bob\1970\197001\19700115.txt
            string path = FileHandlingHelper.CreateDatedUserDirectory(UserModel.Username, filenameDate);
            FileHandlingHelper.TryWriteDataToFile(path, filename, saveData, "o", true, UserModel.Key);
        }
    }
}
