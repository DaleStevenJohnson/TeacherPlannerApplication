using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.Timetable.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class ImportTimetableWindowViewModel : ObservableObject
    {

        private string _timetableName;
        private string _timetableFile;// = FilesAndDirectories.SavedTimetableFileName;
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
            TimetableFile = string.Empty;
            TimetableName = string.Empty;
            UserFeedback = string.Empty;
        }
        public Window Window { get; }
        public UserModel UserModel { get; }
        public static TimetableModel CurrentTimetable { get; set; }
        public string TimetableName
        {
            get => _timetableName;
            set
            {
                //value = ParseTimetableName(value);
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
            {
                TimetableFile = openFileDialog.FileName;
                TimetableName = FilesAndDirectories.SavedTimetableFileName;
                //TimetableName = Path.GetFileName(openFileDialog.FileName);
            }
        }
        private string ParseTimetableName(string name)
        {
            string[] forbiddenCharacters = new string[] { "/", "`", @"\", "~", "#", "*", "\"", "'", ":", ";", ",", "?", "%", "$", "£", "=","+" };
            for (int i = 0; i < forbiddenCharacters.Length; i++)
            {
                name = name.Replace(forbiddenCharacters[i], string.Empty);
            }
            return name;
        }
        private void TryImportTimetable(object args)
        {
            // AND TimetableName is not already an existing Timetable name
            if (TimetableName == string.Empty)
            {
                UserFeedback = "You have not given your timetable a name";
            }
            else if (TimetableFile == string.Empty)
            {
                UserFeedback = "You have not selected a Timetable File";
            }
            else if (TryImportTimetable(TimetableFile, TimetableName, UserModel))
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
        /// Takes a SIMS created timetable file, parses it, converts it 
        /// and if both are successful, it saves the file locally for future use by the application
        /// </summary>
        /// <param name="timetableFilePath"></param>
        public bool TryImportTimetable(string timetableFilePath, string name, UserModel userModel)
        {
            string[][] rawTimetableFileData = FileHandlingHelper.ReadDataFromCSVFile(timetableFilePath);
            if (TryParseTimetableFileData(rawTimetableFileData))
            {
                string[][] convertedTimetableData = ConvertTimetableData(rawTimetableFileData);
                string path = Path.Combine(FileHandlingHelper.LoggedInUserDataPath, FilesAndDirectories.TimetableDirectory);
                FileHandlingHelper.TryWriteDataToCSVFile(path, FilesAndDirectories.SavedTimetableFileName, convertedTimetableData, "o", true, userModel.Key);
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
            // 2 weeks, 5 days, 9 periods + 1 header row
            string[][] convertedTimetableData = new string[(2*5*9)+1][];
            int row = 0;
            convertedTimetableData[row] = new string[] { "Week", "Day", "Period", "Code", "Class", "Room"};
            int column = 0;
            int weeks = 2;
            string[] days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            int[] periodLocations = new int[] {5, 7, 9,11, 12,14, 15, 17,19 };
            for (int week = 1; week <= weeks; week++)
            {
                for (int day = 0; day < days.Length; day++)
                {
                    column += 1;
                    for (int period = 0; period < periodLocations.Length; period++)
                    {
                        row += 1;
                        var periodLocation = periodLocations[period];

                        var weekString = week.ToString();
                        var dayString = days[day];
                        var periodString = rawTimetableData[periodLocation][0];
                        var codeString = weekString + dayString + periodString;
                        
                        
                        var classCode = rawTimetableData[periodLocation][column];
                        if (classCode.Length > 0)
                            classCode = classCode.Substring(0, classCode.Length - 1);

                        var roomNumber = periodLocation != 11 && periodLocation != 14 ? rawTimetableData[periodLocation + 1][column] : "";
                        if (roomNumber.Length > 0)
                            roomNumber = roomNumber.Substring(0, roomNumber.Length - 1);
                        
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
            bool thirdline = timetableFileData[2][0] != string.Empty;
            bool fourthline = ParseEmptyStringArray(timetableFileData[3]);
            bool fifthline = timetableFileData[4][1] == "1Mon";
            bool sixthline = timetableFileData[5][0] == "R";
            bool seventhline = timetableFileData[6][0] == string.Empty && timetableFileData[6][1] != string.Empty;
            bool eigthline = timetableFileData[7][0] == "1";
            bool ninthline = timetableFileData[8][0] == string.Empty && timetableFileData[8][1] != string.Empty;
            bool tenthline = timetableFileData[9][0] == "2";
            bool eleventhline = timetableFileData[10][0] == string.Empty && timetableFileData[10][1] != string.Empty;
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
                if (stringArray[i] != string.Empty)
                    return false;
            }
            return true;
        }
        private void TryLoadTimetable(string timetableName)
        {
            var filepath = Path.Combine(FileHandlingHelper.LoggedInUserDataPath, FilesAndDirectories.TimetableDirectory, timetableName);
            var timetableData = FileHandlingHelper.ReadDataFromCSVFile(filepath, true, UserModel.Key);
            CurrentTimetable = new TimetableModel(timetableData);
        }
    }

}
