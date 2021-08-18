using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.Planner.ViewModels;
using TeacherPlanner.Planner.Views.SettingsWindows;
using TeacherPlanner.Timetable.Models;

namespace TeacherPlanner.Timetable.ViewModels
{
    public class TimetableViewModel : ObservableObject
    {
        private TimetableModel _currentTimetable;
        private int _selectedWeek;

        public TimetableViewModel(UserModel userModel)
        {
            ImportTimetableCommand = new SimpleCommand(_ => OnTimetableImportClick());
            ApplySelectedTimetableCommand = new SimpleCommand(timetableName => ApplySelectedTimetable((string) timetableName));

            UserModel = userModel;
            SelectedWeek = 1;

            TryGetImportedTimetable();
        }
        public int SelectedWeek 
        {
            get => _selectedWeek;
            set => RaiseAndSetIfChanged(ref _selectedWeek, value);
        }
        public UserModel UserModel { get; }
        public TimetableModel CurrentTimetable
        {
            get => _currentTimetable;
            set => RaiseAndSetIfChanged(ref _currentTimetable, value);  
        }

        
        public ICommand ImportTimetableCommand { get; }
        public ICommand ManageTimetableCommand { get; }
        public ICommand ApplySelectedTimetableCommand { get; }

        //TODO implement timetable shift - add button to view
        //TODO Add total Occurances for periods - move dict count to import
        
        private void ChangeWeek()
        {
            SelectedWeek = SelectedWeek == 1 ? 2 : 1;
        }
        private void OnTimetableImportClick()
        {
            var importWindow = new ImportTimetableWindow();
            var importTimetableViewModel = new ImportTimetableWindowViewModel(UserModel, importWindow);
            importWindow.DataContext = importTimetableViewModel;

            if (importWindow.ShowDialog() ?? true)
                TryGetImportedTimetable();
        }
        
        public bool? DefineTimetableWeeks(UserModel userModel)
        {
            var defineTimetableWeeksWindow = new DefineTimetableWeeksWindow(userModel);
            return defineTimetableWeeksWindow.ShowDialog();
        }

        private bool TryGetImportedTimetable()
        {
            var directory = Path.Combine(FileHandlingHelper.LoggedInUserDataPath, FileHandlingHelper.EncryptFileOrDirectory(FilesAndDirectories.TimetableDirectory, UserModel.Key));
            Directory.CreateDirectory(directory);
            var filenames = Directory.GetFiles(directory);

            for (var i = 0; i < filenames.Length; i++)
            {
                var filename = Path.GetFileName(filenames[i]);
                if (filename == FileHandlingHelper.EncryptFileOrDirectory(FilesAndDirectories.SavedTimetableFileName, UserModel.Key))
                {
                    var timetableData = FileHandlingHelper.ReadDataFromCSVFile(filenames[i], true, UserModel.Key);
                    CurrentTimetable = new TimetableModel(timetableData);
                    return true;
                }
            }
            return false;
        }

        private void ApplySelectedTimetable(string filename)
        {
            MessageBox.Show(filename);
        }
    }
}
