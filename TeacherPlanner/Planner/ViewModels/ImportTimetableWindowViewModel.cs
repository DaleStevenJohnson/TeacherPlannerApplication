using Microsoft.Win32;
using System.IO;
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
        public ICommand ChooseTimetableFileCommand { get; }
        public ICommand TryImportTimetableCommand { get;  }

        public ImportTimetableWindowViewModel(UserModel userModel)
        {
            // Parameter Assignment
            UserModel = userModel;

            // Property Initialisation
            ChooseTimetableFileCommand = new SimpleCommand(_ => ChooseTimetableFile(_));
            TryImportTimetableCommand = new SimpleCommand(_ => TryImportTimetable(_));
            TimetableFile = "";
            TimetableName = "";
        }

        public UserModel UserModel { get; }
        public static TimetableModel CurrentTimetable { get; set; }
        public string TimetableName 
        {
            get => _timetableName;
            set => RaiseAndSetIfChanged(ref _timetableName, value);
        }

        public string TimetableFile
        {
            get => _timetableFile;
            set => RaiseAndSetIfChanged(ref _timetableFile, value);
        }
        private void ChooseTimetableFile(object args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Data files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
                TimetableFile = openFileDialog.FileName;
        }
        
        private void TryImportTimetable(object args)
        {
            // AND TimetableName is not already an existing Timetable name
            if (TimetableName != "" && TimetableFile != "" && ImportTimetable(TimetableFile, TimetableName, UserModel))
            {
                TryLoadTimetable(TimetableName);
                // Close Window
            }
            else 
            { 
                // Feedback to user
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
            string[][] convertedTimetableData = new string[rawTimetableData.Length][];
            for (int row = 0; row < rawTimetableData.Length; row++)
            {
                string[] line = rawTimetableData[row];
                if (line.Length != 0)
                {
                    convertedTimetableData[row] = line;
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
