using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class ImportTimetableWindowViewModel : ObservableObject
    {
        
        private string _timetableName;
        private string _timetableFile;
        private string _timetableDirectory = "Timetables";
        private string _userFeedback;

        public ICommand ChooseTimetableFileCommand { get; }
        public ICommand TryImportTimetableCommand { get;  }

        public ImportTimetableWindowViewModel(UserModel userModel, Window window)
        {
            // Parameter Assignment
            UserModel = userModel;
            Window = window;

            // Property Initialisation
            ChooseTimetableFileCommand = new SimpleCommand(_ => ChooseTimetableFile(_));
            TryImportTimetableCommand = new SimpleCommand(_ => TryImportTimetable(_));
            TimetableFile = "";
            TimetableName = "";
            UserFeedback = "";
        }
        public Window Window;
        public UserModel UserModel { get; }
        public static TimetableModel CurrentTimetable { get; set; }
        public string TimetableName
        {
            get => _timetableName;
            set
            {
                value = ParseTimetableName(value);
                RaiseAndSetIfChanged(ref _timetableName, value);
            }
        }

        public string TimetableFile
        {
            get => _timetableFile;
            set => RaiseAndSetIfChanged(ref _timetableFile, value);
        }
        public string UserFeedback
        {
            get => _userFeedback;
            set => RaiseAndSetIfChanged(ref _userFeedback, value);
        }
        private void ChooseTimetableFile(object args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Data files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
                TimetableFile = openFileDialog.FileName;
        }
        private string ParseTimetableName(string name)
        {
            string[] forbiddenCharacters = new string[] { "/", ".", "`", @"\", "~", "#", "*", "\"", "'", ":", ";", ",", "?", "%", "$", "£", "=","+" };
            for (int i = 0; i < forbiddenCharacters.Length; i++)
            {
                name = name.Replace(forbiddenCharacters[i], "");
            }
            return name;
        }
        private void TryImportTimetable(object args)
        {
            // AND TimetableName is not already an existing Timetable name
            if (TimetableName == "")
            {
                UserFeedback = "You have not given your timetable a name";
            }
            else if (TimetableFile == "")
            {
                UserFeedback = "You have not selected a Timetable File";
            }
            else if (ImportTimetable(TimetableFile, TimetableName, UserModel))
            {
                TryLoadTimetable(TimetableName);
                Window.DialogResult = true;
                Window.Close();
            }
            else
            {
                UserFeedback = "Unable to Import Timetable from the given file, are you sure it is in the correct format?";
                // TODO: Add hyperlink to help page 
            }
        }
        /// <summary>
        /// Takes a SIMS created timetable file and saves it locally for future use by the application
        /// </summary>
        /// <param name="timetableFilePath"></param>
        public bool ImportTimetable(string timetableFilePath, string name, UserModel userModel)
        {
            string[][] rawTimetableFileData = FileHandlingHelper.ReadDataFromCSVFile(timetableFilePath);
            if (TryParseTimetableFileData(rawTimetableFileData))
            {
                string[][] convertedTimetableData = ConvertTimetableData(rawTimetableFileData);
                string path = Path.Combine(FileHandlingHelper.UserDataPath, userModel.Username, _timetableDirectory);
                FileHandlingHelper.TryWriteDataToCSVFile(path, name + ".csv", convertedTimetableData, "o", true, userModel.Key);
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Takes a SIMS created timetable, strips out neccessary data and formats it in to the application's standard.
        /// </summary>
        /// <param name="rawTimetableData"></param>
        /// <returns></returns>
        private string[][] ConvertTimetableData(string[][] rawTimetableData)
        {
            // This is unfinished! I have no idea what format the rawTimetableData will be in yet
            string[][] convertedTimetableData = new string[(2*5*5)+1][];
            int row = 0;
            convertedTimetableData[row] = new string[] { "Week", "Day", "Period", "Code", "Class", "Room"};
            int column = 0;
            int weeks = 2;
            string[] days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            int[] periodLocations = new int[] { 7, 9, 12, 15, 17 };
            for (int week = 1; week <= weeks; week++)
            {
                for (int day = 0; day < days.Length; day++)
                {
                    column += 1;
                    for (int period = 0; period < periodLocations.Length; period++)
                    {
                        row += 1;
                        string weekString = week.ToString();
                        string dayString = days[day];
                        string periodString = (period + 1).ToString();
                        string codeString = weekString + dayString + periodString;
                        string classCode = rawTimetableData[periodLocations[period]][column];
                        string roomNumber = rawTimetableData[periodLocations[period] + 1][column];
                        convertedTimetableData[row] = new string[] {weekString, dayString, periodString, codeString, classCode, roomNumber};
                        
                    }
                }
                
            }

            return convertedTimetableData;
        }

        /// <summary>
        /// Checks that provided SIMS timetable data file is in the correct format
        /// </summary>
        /// <param name="timetableFileData"></param>
        /// <returns></returns>
        private bool TryParseTimetableFileData(string[][] timetableFileData)
        {
            // This is probably not the most concise and elegant way to write this, but shows clearly each check performed on each line
            // of the csv data, so arguably is okay? right?
            bool firstline = ParseEmptyStringArray(timetableFileData[0]);
            bool secondline = timetableFileData[1][0].Contains("Timetable");
            bool thirdline = timetableFileData[2][0] != "";
            bool fourthline = ParseEmptyStringArray(timetableFileData[3]);
            bool fifthline = timetableFileData[4][1] == "1Mon";
            bool sixthline = timetableFileData[5][0] == "R";
            bool seventhline = timetableFileData[6][0] == "" && timetableFileData[6][1] != "";
            bool eigthline = timetableFileData[7][0] == "1";
            bool ninthline = timetableFileData[8][0] == "" && timetableFileData[8][1] != "";
            bool tenthline = timetableFileData[9][0] == "2";
            bool eleventhline = timetableFileData[10][0] == "" && timetableFileData[10][1] != "";
            bool twentiethline = timetableFileData[19][0] == "T";
            bool twentyfirstline = ParseEmptyStringArray(timetableFileData[20]);
            bool twentysecondline = ParseEmptyStringArray(timetableFileData[21]);
            bool twentythirdline = ParseEmptyStringArray(timetableFileData[22]);
            // Sam will potentially laugh in disgust at the below return. If it's stoopid && it works, it's !stoopid...
            // I am hoping there is a way to make the boolean expression more concise... Maybe using a list? Add each check to the list, instead of an individually named variable
            // then at the end iterate through and && all values together...
            return firstline && secondline && thirdline && fourthline && fifthline && sixthline && seventhline && eigthline && ninthline && tenthline && eleventhline && twentiethline && twentyfirstline && twentysecondline && twentythirdline;
        }
        private bool ParseEmptyStringArray(string[] stringArray)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (stringArray[i] != "")
                    return false;
            }
            return true;
        }
        private void TryLoadTimetable(string timetableName)
        {
            var filepath = Path.Combine(FileHandlingHelper.LoggedInUserDataPath, _timetableDirectory, timetableName + ".csv");
            var timetableData = FileHandlingHelper.ReadDataFromCSVFile(filepath, true, UserModel.Key);
            CurrentTimetable = new TimetableModel(timetableData, timetableName);
        }
    }

}
