using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Database;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class DefineTimetableWeeksViewModel : ObservableObject
    {
        private ObservableCollection<TimetableWeekModel> _timetableWeeks;
        private readonly AcademicYearModel _academicYearModel;
        private readonly UserModel _userModel;
        public ICommand SaveTimeTableWeeksCommand { get; }
        public DefineTimetableWeeksViewModel(UserModel userModel, AcademicYearModel academicYear)
        {
            // Parameter Assignment
            _userModel = userModel;
            _academicYearModel = academicYear;

            SaveTimeTableWeeksCommand = new SimpleCommand(window => OnSaveTimeTableWeeks((Window)window));

            if (TryGetTimeTableWeeks(out var dateRows))
                TimetableWeeks = dateRows;
            else
                TimetableWeeks = CreateTimeTableWeeks();
        }
        
        // Properties

        public ObservableCollection<TimetableWeekModel> TimetableWeeks
        {
            get => _timetableWeeks;
            set => RaiseAndSetIfChanged(ref _timetableWeeks, value);
        }

        
        // Methods
        private bool TryGetTimeTableWeeks(out ObservableCollection<TimetableWeekModel> timetableWeeks)
        {
            var dbModels = DatabaseManager.GetTimetableWeeks(_academicYearModel.ID);

            timetableWeeks = new ObservableCollection<TimetableWeekModel>(
                dbModels
                .ConvertAll(w => new TimetableWeekModel(w.ID, w.WeekBeginning, w.Week)));

            return timetableWeeks.Any();
        }
        
        private ObservableCollection<TimetableWeekModel> CreateTimeTableWeeks()
        {
            var schoolYear = GetSchoolYear();
            DateTime date = GetFirstMonday(schoolYear);
            int weeks = 50;
            var timetableWeeks = new ObservableCollection<TimetableWeekModel>();

            for (int i = 0; i < weeks; i++)
            {
                var weekBeginning = date.AddDays(i * 7);

                var dbModel = new TimetableWeek()
                { 
                    AcademicYearID = _academicYearModel.ID,
                    WeekBeginning = weekBeginning,
                    Week = 0,
                };

                if (DatabaseManager.TryAddTimetableWeek(dbModel, out var id))
                    timetableWeeks.Add(new TimetableWeekModel(id, weekBeginning));
            }

            return timetableWeeks;
        }
  
        private DateTime GetFirstMonday(int schoolYear)
        {
            DateTime date = new DateTime(schoolYear, 9, 1);
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(1);
            }
            return date.AddDays(-7);
        }

        private int GetSchoolYear()
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;
            return month < 8 ? year - 1 : year;
        }

        private void OnSaveTimeTableWeeks(Window window)
        {
            SaveTimeTableWeeks();
            window.DialogResult = true;
            window.Close();
        }
        private void SaveTimeTableWeeks()
        {
            foreach (var week in TimetableWeeks)
            {
                var dbModel = new TimetableWeek()
                {
                    ID = week.ID,
                    AcademicYearID = _academicYearModel.ID,
                    WeekBeginning = week.Date,
                    Week = (int)week.Week
                };

                DatabaseManager.TryUpdateTimetableWeek(dbModel);
            }
        }
    }
}
