using System;
using System.Windows;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.Models;

namespace TeacherPlanner.Planner.ViewModels
{
    public class PlannerViewModel
    {
        private const int DAYLIMIT = 14;
        private DayViewModel _leftDay;
        private DayViewModel _rightDay;
        private int _loadedDayModelsIndex = 0;
        public PlannerViewModel(UserModel userModel, TimetableModel timetable)
        {
            // Parameter Assignment
            UserModel = userModel;
            Timetable = timetable;

            // Property Assignment
            LoadedDayModels = new DayModel[DAYLIMIT];
            LeftDay = new DayViewModel(UserModel, CalendarHelper.CurrentDateLeft, Timetable);
            RightDay = new DayViewModel(UserModel, CalendarHelper.CurrentDateRight, Timetable);


            LoadNewDays();

            // Command Assignment
            //TurnPageForwardCommand = new SimpleCommand(numOfDays => OnTurnPageForward(Convert.ToInt32(numOfDays)));
            //TurnPageBackwardCommand = new SimpleCommand(numOfDays => OnTurnPageBackward(Convert.ToInt32(numOfDays)));

            LeftDay.TurnPageEvent += (_, __) => OnTurnPage(__);
            RightDay.TurnPageEvent += (_, __) => OnTurnPage(__);
            SaveCommand = new SimpleCommand(_ => OnSave());
        }
        public TimetableModel Timetable { get; set; }
        public DayModel[] LoadedDayModels { get; set; }

        public UserModel UserModel { get; set; }
        public DayViewModel LeftDay
        {
            get => _leftDay;
            set => _leftDay = value;
        }
        public DayViewModel RightDay
        {
            get => _rightDay;
            set => _rightDay = value;
        }

        public ICommand SaveCommand { get; }
        public void OnTurnPage(object parameter)
        {
            var numOfDays = int.Parse((string)parameter);
            SaveCurrentlyDisplayedPageDays();
            CalendarHelper.ChangeCurrentDate(numOfDays);
            LoadNewDays();
            //_debug.Text = $"{LeftDay.Period1.Row1.LeftText}";
        }
        public void OnSave()
        {
            SaveCurrentlyDisplayedPageDays();
            MessageBox.Show("Saved");
        }

        private void SaveCurrentlyDisplayedPageDays()
        {
            LeftDay.SaveDayToFile();
            RightDay.SaveDayToFile();
        }

        private void LoadNewDays()
        {
            LoadNewDay(CalendarHelper.CurrentDateLeft, ref _leftDay);
            LoadNewDay(CalendarHelper.CurrentDateRight, ref _rightDay);
        }

        private void LoadNewDay(DateTime date, ref DayViewModel dayViewModel)
        {
            int index = IndexOfLoadedDay(date);
            if (index != -1)
                dayViewModel.DayModel = LoadedDayModels[index];
            else
            {
                dayViewModel.DayModel = dayViewModel.LoadAndPopulateNewDay(date);
                AddDayToLoadedDayModelList(dayViewModel.DayModel);
            }
        }
        private int IndexOfLoadedDay(DateTime date)
        {
            for (int i = 0; i < LoadedDayModels.Length; i++)
            {
                if (LoadedDayModels[i] != null && LoadedDayModels[i].CalendarModel.Date == date)
                    return i;
            }
            return -1;
        }
        private void AddDayToLoadedDayModelList(DayModel day)
        {
            LoadedDayModels[_loadedDayModelsIndex] = day;
            _loadedDayModelsIndex++;
            if (_loadedDayModelsIndex >= DAYLIMIT)
            {
                _loadedDayModelsIndex = 0;
            }
        }
        private void MoveLeft()
        {
            for (int i = 1; i < LoadedDayModels.Length; i++)
            {
                LoadedDayModels[i - 1] = LoadedDayModels[i];
            }
        }
        private void MoveRight()
        {
            for (int i = LoadedDayModels.Length - 2; i > -1; i--)
            {
                LoadedDayModels[i + 1] = LoadedDayModels[i];
            }
        }



    }


}
