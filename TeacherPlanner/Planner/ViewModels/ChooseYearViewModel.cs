using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
    public class ChooseYearViewModel : ObservableObject
    {
        private ObservableCollection<YearSelectModel> _yearSelectModels;
        public event EventHandler<string> ChooseYearEvent;
        public ICommand AddYearCommand { get; }
        public ChooseYearViewModel(UserModel usermodel)
        {
            // Parameter Assignments
            UserModel = usermodel;

            // Property Assignments
            YearSelectModels = GetAcademicYears(); ; //new List<YearSelectModel>();


            //Command Assignments
            AddYearCommand = new SimpleCommand(_ => OnAddYear());

        }
        private UserModel UserModel { get; set; }
        public ObservableCollection<YearSelectModel> YearSelectModels
        {
            get => _yearSelectModels;
            set => RaiseAndSetIfChanged(ref _yearSelectModels, value);
        }

        // Public Methods

        public void OnYearSelected(object v)
        {
            ChooseYearEvent.Invoke(null, (string)v);
        }

        public YearSelectModel AddNewAcademicYear()
        {
            var year = GetNextAcademicYear();
            if (!DatabaseManager.CheckIfAcademicYearExists(UserModel.ID, year))
            {
                // Adding of a new academic year
                var newYear = new YearSelectModel(year);
                var academicYear = new AcademicYear()
                {
                    UserID = UserModel.ID,
                    Year = newYear.Year
                };
                if (DatabaseManager.TrySaveAcademicYear(academicYear))
                    return newYear;
            }
            return null;
        }

        // Private Methods

        private int GetNextAcademicYear()
        {
            // Work out which academic year to add
            if (YearSelectModels == null || !YearSelectModels.Any())
                return CalendarManager.GetStartingYearOfAcademicYear(DateTime.Today);
            else
            {
                return YearSelectModels
                    .OrderBy(y => y.Year)
                    .ToList()
                    .Last()
                    .Year + 1;
            }
                
        }

        private void OnAddYear()
        {
            var newYear = AddNewAcademicYear();
            if (newYear != null)
                YearSelectModels.Add(newYear);
        }

        private ObservableCollection<YearSelectModel> GetAcademicYears()
        {
            var yearSelectModels = new ObservableCollection<YearSelectModel>();
            // Get currently stored Academic Years from database
            var academicYears = DatabaseManager.GetAcademicYears(UserModel.ID);
            
            if (!academicYears.Any())
            {
                var newYear = AddNewAcademicYear();
                if (newYear != null)
                    yearSelectModels.Add(newYear);
            }
            else
            {
                foreach (var year in academicYears)
                {
                    var newYear = new YearSelectModel(year.Year);
                    yearSelectModels.Add(newYear);
                }
            }
            return yearSelectModels;
        }
    }
}
