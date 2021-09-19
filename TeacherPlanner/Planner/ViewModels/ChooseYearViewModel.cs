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
        public event EventHandler<YearSelectModel> ChooseYearEvent;
        public ICommand AddYearCommand { get; }
        public ICommand SelectYearCommand { get; }
        public ChooseYearViewModel(UserModel usermodel)
        {
            // Parameter Assignments
            UserModel = usermodel;

            // Property Assignments
            YearSelectModels = GetAcademicYears(); ; //new List<YearSelectModel>();


            //Command Assignments
            AddYearCommand = new SimpleCommand(_ => OnAddNewAcademicYear());
            SelectYearCommand = new SimpleCommand(_ => OnSelectYear(_));

        }
        private UserModel UserModel { get; set; }
        public ObservableCollection<YearSelectModel> YearSelectModels
        {
            get => _yearSelectModels;
            set => RaiseAndSetIfChanged(ref _yearSelectModels, value);
        }

        // Public Methods

        public void OnSelectYear(object v)
        {
            ChooseYearEvent.Invoke(null, (YearSelectModel)v);
        }

        // Private Methods

        private void OnAddNewAcademicYear()
        {
            var year = GetNextAcademicYear();

            if (!DatabaseManager.CheckIfAcademicYearExists(UserModel.ID, year))
            {
                
                var academicYear = new AcademicYear()
                {
                    UserID = UserModel.ID,
                    Year = year
                };

                if (DatabaseManager.TrySaveAcademicYear(academicYear))
                    YearSelectModels = GetAcademicYears();
            }
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


        private ObservableCollection<YearSelectModel> GetAcademicYears()
        {
            var yearSelectModels = new ObservableCollection<YearSelectModel>();
            // Get currently stored Academic Years from database
            var academicYears = DatabaseManager.GetAcademicYears(UserModel.ID);
            
            if (!academicYears.Any())
            {
                OnAddNewAcademicYear();
            }
            else
            {
                foreach (var year in academicYears)
                {
                    var newYear = new YearSelectModel(year.Year, year.ID);
                    yearSelectModels.Add(newYear);
                }
            }
            return yearSelectModels;
        }
    }
}
