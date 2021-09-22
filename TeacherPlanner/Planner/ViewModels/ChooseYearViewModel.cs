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
        private ObservableCollection<AcademicYearModel> _yearSelectModels;
        public event EventHandler<AcademicYearModel> ChooseYearEvent;
        public ICommand AddYearCommand { get; }
        public ICommand SelectYearCommand { get; }
        public ChooseYearViewModel(UserModel usermodel)
        {
            // Parameter Assignments
            UserModel = usermodel;

            // Property Assignments
            YearSelectModels = GetAcademicYears();
            if (!YearSelectModels.Any())
                OnAddNewAcademicYear();

            //Command Assignments
            AddYearCommand = new SimpleCommand(_ => OnAddNewAcademicYear());
            SelectYearCommand = new SimpleCommand(_ => OnSelectYear(_));

        }
        private UserModel UserModel { get; set; }
        public ObservableCollection<AcademicYearModel> YearSelectModels
        {
            get => _yearSelectModels;
            set => RaiseAndSetIfChanged(ref _yearSelectModels, value);
        }

        // Public Methods

        public void OnSelectYear(object v)
        {
            ChooseYearEvent.Invoke(null, (AcademicYearModel)v);
        }

        // Private Methods

        private void OnAddNewAcademicYear()
        {
            if (AddAcademicYearToDatabase())   
                YearSelectModels = GetAcademicYears();
        }

        private bool AddAcademicYearToDatabase()
        {
            var year = GetNextAcademicYear();
            
            if (!DatabaseManager.CheckIfAcademicYearExists(UserModel.ID, year))
            {
                var academicYear = new AcademicYear()
                {
                    UserID = UserModel.ID,
                    Year = year
                };
                return DatabaseManager.TryAddAcademicYear(academicYear);
            }
            return false;
        }

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

        private ObservableCollection<AcademicYearModel> GetAcademicYears()
        {
            var yearSelectModels = new ObservableCollection<AcademicYearModel>();
            // Get currently stored Academic Years from database
            var academicYears = DatabaseManager.GetAcademicYears(UserModel.ID);
            
            if (academicYears.Any())
            {
                foreach (var year in academicYears)
                {
                    var newYear = new AcademicYearModel(year.Year, year.ID);
                    yearSelectModels.Add(newYear);
                }
            }
            return yearSelectModels;
        }
    }
}
