using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class ChooseYearViewModel : ObservableObject
    {
        private List<YearSelectModel> _yearSelectModels;
        public event EventHandler<string> ChooseYearEvent;
        public ICommand AddYearCommand { get; }
        public ChooseYearViewModel(UserModel usermodel)
        {
            UserModel = usermodel;
            AddYearCommand = new SimpleCommand(_ => AddYear());
            List<YearSelectModel> YearSelectModels = new List<YearSelectModel>();
            GetYears();
        }
        private UserModel UserModel { get; set; }
        public List<YearSelectModel> YearSelectModels 
        {
            get => _yearSelectModels;
            set => RaiseAndSetIfChanged(ref _yearSelectModels, value);
        }
        public void OnYearSelected(object v)
        {
            ChooseYearEvent.Invoke(null, (string)v);
        }
        private string GetCurrentYear()
        {
            var day = Int32.Parse(DateTime.Today.ToString("dd"));
            var month = Int32.Parse(DateTime.Today.ToString("MM"));
            var year = Int32.Parse(DateTime.Today.ToString("yyyy"));
            if ((month == 8 && day <= 15) || month > 1 && month < 8)
                return (year - 1).ToString();
            else
                return year.ToString();
        }
        private void GetYears()
        {
            List<YearSelectModel> yearSelectModels = new List<YearSelectModel>();
            string path = Path.Combine(FileHandlingHelper.UserDataPath, UserModel.UsernameHash);
            string[] currentYears = Directory.GetDirectories(path);
            if (currentYears.Length == 0)
            {
                YearSelectModel newYear = new YearSelectModel(CalendarManager.GetStartingYearOfAcademicYear(DateTime.Today));
                yearSelectModels.Add(newYear);
                Directory.CreateDirectory(Path.Combine(path, newYear.AcademicYear));
            }
            else
            {
                for (int i = 0; i < currentYears.Length; i++)
                {
                    var name = Path.GetFileName(currentYears[i]);
                    YearSelectModel newYear = new YearSelectModel(name.Substring(0, 4));
                    yearSelectModels.Add(newYear);
                }
            }
            YearSelectModels = yearSelectModels;
        }
        public void AddYear()
        {
            List<YearSelectModel> yearSelectModels;
            if (YearSelectModels != null)
                yearSelectModels = new List<YearSelectModel>(YearSelectModels);
            else
                yearSelectModels = new List<YearSelectModel>();
            string path = Path.Combine(FileHandlingHelper.UserDataPath, UserModel.UsernameHash);
            string[] currentYears = Directory.GetDirectories(path);
            YearSelectModel newYear;
            if (currentYears.Length == 0)
            {
                newYear = new YearSelectModel(CalendarManager.GetStartingYearOfAcademicYear(DateTime.Today));
                yearSelectModels.Add(newYear);
            }
            else
            {
                string finalYear = Path.GetFileName(currentYears[currentYears.Length - 1]);
                int yearToAdd = Int32.Parse(finalYear.Substring(0, 4)) + 1;
                newYear = new YearSelectModel(yearToAdd.ToString());
                yearSelectModels.Add(newYear);
            }
            Directory.CreateDirectory(Path.Combine(path, newYear.AcademicYear));
            YearSelectModels = yearSelectModels;
        }
    }
}
