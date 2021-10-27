using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.PlannerYear.Models;

namespace TeacherPlanner.AcademicYearConfiguration.ViewModels
{
    public class AcademicYearConfigurationViewModel
    {
        private readonly int MAX_PERIODS = 10;
        private readonly int MAX_TIMETABLE_WEEKS = 2;

        public ICommand ConfirmConfigurationCommand { get; }
        public ICommand CancelConfigurationCommand { get; }

        public event EventHandler ConfirmConfigurationEvent;
        public event EventHandler CancelConfigurationEvent;

        
        public AcademicYearConfigurationViewModel()
        {
            // Parameter Asssignment

            
            // Property Assignment
            ComboBoxPeriodList = PopulateListWithIntegers(MAX_PERIODS);
            TimetableWeeksList = PopulateListWithIntegers(MAX_TIMETABLE_WEEKS);

            // Command Assignment
            ConfirmConfigurationCommand = new SimpleCommand(_ => OnConfirmConfiguration());
            CancelConfigurationCommand = new SimpleCommand(_ => OnCancelConfiguration());
            
            // Event Subscription

            // Method Calls


        }
        public AcademicYearModel CurrentAcademicYear { get; set; }
        public List<int> ComboBoxPeriodList { get; }
        public List<int> TimetableWeeksList { get; }

        private void OnConfirmConfiguration()
        {
            ConfirmConfigurationEvent.Invoke(null, EventArgs.Empty);
        }

        private void OnCancelConfiguration()
        {
            CancelConfigurationEvent.Invoke(null, EventArgs.Empty);
        }

        private List<int> PopulateListWithIntegers(int range)
        {
            var intList = new List<int>();
            
            for (int i = 1; i <= range; i++)
            {
                intList.Add(i);
            }
            
            return intList;
        }
    }
}
