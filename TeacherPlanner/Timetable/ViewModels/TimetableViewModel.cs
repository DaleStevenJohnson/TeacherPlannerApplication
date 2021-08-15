using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.Planner.ViewModels;
using TeacherPlanner.Planner.Views.SettingsWindows;

namespace TeacherPlanner.Timetable.ViewModels
{
    public class TimetableViewModel : ObservableObject
    {
        private static string _timetableDirectory = "Timetables";
        private bool _hasImportedTimetables;
        private List<TimetableModel> _importedTimetables;
        private UserModel _userModel;

        public TimetableViewModel(UserModel userModel)
        {
            ImportTimetableCommand = new SimpleCommand(_ => OnTimetableImportClick());
            DefineTimetableWeeksCommand = new SimpleCommand(_ => OnDefineTimetableWeeks());
            ManageTimetableCommand = new SimpleCommand(_ => OnManageTimetable());
            ApplySelectedTimetableCommand = new SimpleCommand(timetableName => ApplySelectedTimetable((string) timetableName));
            _userModel = userModel;
            GetImportedTimetables();
        }

        public List<TimetableModel> ImportedTimetables 
        { 
            get => _importedTimetables;
            set => RaiseAndSetIfChanged(ref _importedTimetables, value);  
        }

        public ICommand DefineTimetableWeeksCommand { get; }
        public ICommand ImportTimetableCommand { get; }
        public ICommand ManageTimetableCommand { get; }
        public ICommand ApplySelectedTimetableCommand { get; }

        public bool HasImportedTimetables
        {
            get => ImportedTimetables.Count > 0;
            set => RaiseAndSetIfChanged(ref _hasImportedTimetables, value);
        }

        private void OnManageTimetable()
        { 
        
        }
        
        private void OnTimetableImportClick()
        {
            var importWindow = new ImportTimetableWindow();
            var importTimetableViewModel = new ImportTimetableWindowViewModel(_userModel);
            importWindow.DataContext = importTimetableViewModel;

            if (importWindow.ShowDialog() == true)
                GetImportedTimetables();
        }
        
        private bool? OnDefineTimetableWeeks()
        {
            var defineTimetableWeeksWindow = new DefineTimetableWeeksWindow();
            return defineTimetableWeeksWindow.ShowDialog();
        }

        private void GetImportedTimetables()
        {
            string[] filenames = Directory.GetFiles(Path.Combine(FileHandlingHelper.LoggedInUserDataPath, _timetableDirectory));
            var updatedImportedTimetables = new List<TimetableModel>();

            for (int i = 0; i < filenames.Length; i++)
            {
                updatedImportedTimetables.Add(new TimetableModel(new string[0][], Path.GetFileName(filenames[i])));
            }

            ImportedTimetables = updatedImportedTimetables;
        }

        private void ApplySelectedTimetable(string filename)
        {
            MessageBox.Show(filename);
        }
    }
}
