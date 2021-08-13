using Microsoft.Win32;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class ImportTimetableWindowViewModel : ObservableObject
    {
        
        private string _timetableName;
        private string _timetableFile;
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
            if (openFileDialog.ShowDialog() == true)
                TimetableFile = openFileDialog.FileName;
        }
        private void TryImportTimetable(object args)
        {
            // AND TimetableName is not already an existing Timetable name
            if (TimetableName != "" && TimetableFile != "")
            {
                // Close Window
            }
            else 
            { 
                // Feedback to user
            }
        }
    }

}
