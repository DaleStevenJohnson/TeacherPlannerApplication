using Database;
using Database.DatabaseModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        private readonly AcademicYearModel _academicYear;

        public ICommand ChooseTimetableFileCommand { get; }
        public ICommand ImportTimetableCommand { get;  }

        public ImportTimetableWindowViewModel(AcademicYearModel academicYear)
        {
            // Parameter Assignment
            _academicYear = academicYear;

            // Property Initialisation
            TimetableFile = string.Empty;
            TimetableName = string.Empty;
            UserFeedback = string.Empty;

            // Command Assignment
            ChooseTimetableFileCommand = new SimpleCommand(_ => ChooseTimetableFile(_));
            ImportTimetableCommand = new SimpleCommand(window => OnImportTimetable((Window)window));
            
        }
        
        // Properties

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

        // Private Methods

        private void ChooseTimetableFile(object args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Data files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                TimetableFile = openFileDialog.FileName;
                TimetableName = Path.GetFileName(TimetableFile);
            }
        }
     
        private void OnImportTimetable(Window window)
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
            else if (TryImportTimetable(TimetableFile))
            {
                window.DialogResult = true;
                window.Close();
            }
            else
            {
                UserFeedback = "Unable to Import Timetable from the given file, are you sure it is in the correct format?";
                // TODO: Add hyperlink to help page 
            }
        }
        
        private bool TryImportTimetable(string timetableFilePath)
        {
            string[][] rawTimetableFileData = FileHandlingHelper.ReadDataFromCSVFile(timetableFilePath);
            
            if (!TryParseTimetableFileData(rawTimetableFileData))
                return false;
            
            var convertedTimetableData = ConvertTimetableData(rawTimetableFileData);
            foreach(var period in convertedTimetableData)
            {
                if (DatabaseManager.TimetablePeriodIsInDatabase(period, out var id))
                {
                    period.ID = id;
                    if (!DatabaseManager.TryUpdateTimetablePeriod(period))
                        return false;
                }
                else if (!DatabaseManager.TryAddTimetablePeriod(period, out _))
                    return false;
            }
                
            return true;
        }
       
        private List<TimetablePeriod> ConvertTimetableData(string[][] rawTimetableData)
        {
            int row = 0;
            int column = 0;
            int weeks = 2;
            string[] days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            int[] periodLocations = new int[] {5, 7, 9,11, 12,14, 15, 17,19 };

            var timetablePeriods = new List<TimetablePeriod>();

            // 2 weeks
            for (int week = 1; week <= weeks; week++)
            {
                // 5 days
                for (int day = 0; day < days.Length; day++)
                {
                    // 9 periods (6 + Break, Lunch + Twilight)
                    column += 1;
                    for (int period = 0; period < periodLocations.Length; period++)
                    {
                        row += 1;
                        var periodLocation = periodLocations[period];
                        // var periodString = rawTimetableData[periodLocation][0];
                        
                        // SIMS exports with some odd characters at the end of classcode strings. 
                        // This means having to remove the last character from the strings for them to display properly
                        var roomNumber = periodLocation != 11 && periodLocation != 14 ? rawTimetableData[periodLocation + 1][column] : "";
                        if (roomNumber.Length > 0)
                            roomNumber = roomNumber.Substring(0, roomNumber.Length - 1);
                        
                        var classCode = rawTimetableData[periodLocation][column];
                        if (classCode.Length > 0)
                            classCode = classCode.Substring(0, classCode.Length - 1);

                        var timetablePeriod = new TimetablePeriod()
                        {
                            AcademicYearID = _academicYear.ID,
                            Week = week,
                            Day = day + 1,
                            Period = period,
                            ClassCode = classCode,
                            RoomCode = roomNumber,
                        };

                        timetablePeriods.Add(timetablePeriod);
                    }
                }
            }
            return timetablePeriods;
        }

        private bool TryParseTimetableFileData(string[][] timetableFileData)
        {
            // This is probably not the most concise and elegant way to write this, but shows clearly each check performed on each line
            // of the csv data, so arguably is okay? right?
            bool valid = ParseEmptyStringArray(timetableFileData[0])
            && timetableFileData[1][0].Contains("Timetable")
            && timetableFileData[2][0] != string.Empty
            && ParseEmptyStringArray(timetableFileData[3])
            && timetableFileData[4][1] == "1Mon"
            && timetableFileData[5][0] == "R"
            && timetableFileData[6][0] == string.Empty 
            && timetableFileData[6][1] != string.Empty
            && timetableFileData[7][0] == "1"
            && timetableFileData[8][0] == string.Empty 
            && timetableFileData[8][1] != string.Empty
            && timetableFileData[9][0] == "2"
            && timetableFileData[10][0] == string.Empty 
            && timetableFileData[10][1] != string.Empty
            && timetableFileData[19][0] == "T"
            && ParseEmptyStringArray(timetableFileData[20])
            && ParseEmptyStringArray(timetableFileData[21])
            && ParseEmptyStringArray(timetableFileData[22]);
           
            return valid;
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
    }
}
