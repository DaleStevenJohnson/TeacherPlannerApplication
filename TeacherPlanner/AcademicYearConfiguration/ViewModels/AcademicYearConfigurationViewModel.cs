using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TeacherPlanner.AcademicYearConfiguration.Models;
using TeacherPlanner.Constants;
using TeacherPlanner.Helpers;
using TeacherPlanner.PlannerYear.Models;

namespace TeacherPlanner.AcademicYearConfiguration.ViewModels
{
    public class AcademicYearConfigurationViewModel : ObservableObject
    {
        private readonly int MAX_PERIODS = 10;
        private readonly int MAX_BREAKS = 2;
        private readonly int MAX_TIMETABLE_WEEKS = 2;
        private int _lessonCount;
        private int _breakCount;
        private int _lunchCount;
        private int _registrationCount;
        private int _twilightCount;
        private ObservableCollection<PeriodTimingsModel> _periodTimingsModels;

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
        public ObservableCollection<PeriodTimingsModel> PeriodTimingsModels 
        {
            get => _periodTimingsModels;
            private set => RaiseAndSetIfChanged(ref _periodTimingsModels, value); 
        }
        public int LessonCount 
        {
            get => _lessonCount;
            set
            {
                if (RaiseAndSetIfChanged(ref _lessonCount, value))
                {
                    PopulatePeriodTimingsModels("Lesson");
                }
            }
        }

        public int BreakCount
        {
            get => _breakCount;
            set
            {
                if (RaiseAndSetIfChanged(ref _breakCount, value))
                {
                    PopulatePeriodTimingsModels("Break");
                }
            }
        }

        public int LunchCount
        {
            get => _lunchCount;
            set
            {
                if (RaiseAndSetIfChanged(ref _lunchCount, value))
                {
                    PopulatePeriodTimingsModels("Lunch");
                }
            }
        }

        public int RegistrationCount
        {
            get => _registrationCount;
            set
            {
                if (RaiseAndSetIfChanged(ref _registrationCount, value))
                {
                    PopulatePeriodTimingsModels("Registration");
                }
            }
        }
        public int TwilightCount
        {
            get => _twilightCount;
            set
            {
                if (RaiseAndSetIfChanged(ref _twilightCount, value))
                {
                    PopulatePeriodTimingsModels("Twilight");
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

        private void PopulatePeriodTimingsModels(string periodType)
        {

            var extraPeriods = CalculateExtraTimingsModelsToAdd(periodType, out PeriodCodes mostRecentPeriodCode);
            
            if (extraPeriods == 0)
            {
                return;
            }

            if (mostRecentPeriodCode == PeriodCodes.Undefined)
            {
                mostRecentPeriodCode = PeriodCodesConverter.ConvertPeriodTypeToPeriodCodes(periodType);
            }
            else
            {
                mostRecentPeriodCode = mostRecentPeriodCode.Next();
            }

            if (extraPeriods > 0)
            {
                for (int i = 0; i < extraPeriods; i++)
                {
                    var periodTimingModel = new PeriodTimingsModel(periodType, mostRecentPeriodCode);
                    periodTimingModel.TimeUpdatedEvent += (_,__) => SortPeriodTimingsModels();
                    PeriodTimingsModels.Add(periodTimingModel);
                    mostRecentPeriodCode = mostRecentPeriodCode.Next();
                }
            }
            else
            {
                var position = PeriodTimingsModels.Count - 1;
                while (extraPeriods < 0)
                {
                    var model = PeriodTimingsModels[position];
                    if (model.Type == periodType)
                        model.TimeUpdatedEvent -= (_,__) => SortPeriodTimingsModels();
                        PeriodTimingsModels.Remove(model);
                        extraPeriods += 1;
                    position--;
                }
            }


            SortPeriodTimingsModels();
        }

        private void SortPeriodTimingsModels()
        {
            var myComparer = new PeriodTimingsModelComparer();
            var modelList = new List<PeriodTimingsModel>(PeriodTimingsModels);
            modelList.Sort(myComparer);
            PeriodTimingsModels = new ObservableCollection<PeriodTimingsModel>(modelList);
        }

        private int CalculateExtraTimingsModelsToAdd(string periodType, out PeriodCodes mostRecentPeriodCode)
        {
            int countOfExistingPeriodType = CountExistingPeriodTypes(periodType, out PeriodCodes _mostRecentPeriodCode);
            mostRecentPeriodCode = _mostRecentPeriodCode;

            switch (periodType)
            {
                case "Lesson":
                    return LessonCount - countOfExistingPeriodType;
                case "Break":
                    return BreakCount - countOfExistingPeriodType;
                case "Lunch":
                    return LessonCount - countOfExistingPeriodType;
                case "Registration":
                    return LessonCount - countOfExistingPeriodType;
                case "Twilight":
                    return LessonCount - countOfExistingPeriodType;
                default:
                    return 0;
            }
        }

        private int CountExistingPeriodTypes(string periodType, out PeriodCodes mostRecentPeriodCode)
        {
            var counter = 0;
            mostRecentPeriodCode = PeriodCodes.Undefined;
            foreach (var periodTimingModel in PeriodTimingsModels)
            {
                if (periodTimingModel.Type == periodType)
                {
                    counter++;
                    mostRecentPeriodCode = periodTimingModel.PeriodCode;
                }
            }
            return counter;
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
