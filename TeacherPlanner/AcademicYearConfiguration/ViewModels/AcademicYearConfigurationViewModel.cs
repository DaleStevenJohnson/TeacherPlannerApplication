using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.AcademicYearConfiguration.Models;
using TeacherPlanner.Helpers;
using TeacherPlanner.PlannerYear.Models;

namespace TeacherPlanner.AcademicYearConfiguration.ViewModels
{
    public class AcademicYearConfigurationViewModel : ObservableObject
    {
        private readonly int MAX_PERIODS = 10;
        private readonly int MAX_BREAKS = 2;
        private readonly int MAX_TIMETABLE_WEEKS = 2;
        private int _periodCount;

        public ICommand ConfirmConfigurationCommand { get; }
        public ICommand CancelConfigurationCommand { get; }

        public event EventHandler ConfirmConfigurationEvent;
        public event EventHandler CancelConfigurationEvent;

        
        public AcademicYearConfigurationViewModel()
        {
            // Parameter Asssignment

            
            // Property Assignment
            ComboBoxPeriodList = PopulateListWithIntegers(MAX_PERIODS);
            ComboBoxTimetableWeeksList = PopulateListWithIntegers(MAX_TIMETABLE_WEEKS);
            ComboBoxBreaksList = PopulateListWithIntegers(MAX_BREAKS);
            PeriodTimingsModels = new ObservableCollection<PeriodTimingsModel>();

            // Command Assignment
            ConfirmConfigurationCommand = new SimpleCommand(_ => OnConfirmConfiguration());
            CancelConfigurationCommand = new SimpleCommand(_ => OnCancelConfiguration());
            
            // Event Subscription

            // Method Calls


        }
        public AcademicYearModel CurrentAcademicYear { get; set; }
        public List<int> ComboBoxPeriodList { get; }
        public List<int> ComboBoxTimetableWeeksList { get; }
        public List<int> ComboBoxBreaksList { get; }
        public ObservableCollection<PeriodTimingsModel> PeriodTimingsModels { get; private set; }
        public int PeriodCount 
        {
            get => _periodCount;
            set
            {
                if (RaiseAndSetIfChanged(ref _periodCount, value))
                {
                    PopulatePeriodTimingsModels();
                }
            }
        }

        private void OnConfirmConfiguration()
        {
            ConfirmConfigurationEvent.Invoke(null, EventArgs.Empty);
        }

        private void OnCancelConfiguration()
        {
            CancelConfigurationEvent.Invoke(null, EventArgs.Empty);
        }

        private void PopulatePeriodTimingsModels()
        {
            var extraPeriods = PeriodCount - PeriodTimingsModels.Count;
            if (extraPeriods == 0)
            {
                return;
            }
            else if (extraPeriods > 0)
            {
                for (int i = 0; i < extraPeriods; i++)
                {
                    var periodTimingModel = new PeriodTimingsModel();
                    PeriodTimingsModels.Add(periodTimingModel);
                }
            }
            else
            {
                while (extraPeriods < 0)
                {
                    PeriodTimingsModels.RemoveAt(PeriodTimingsModels.Count - 1);
                    extraPeriods += 1;
                }
            }
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
