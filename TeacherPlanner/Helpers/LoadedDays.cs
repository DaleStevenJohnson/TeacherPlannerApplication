using System;
using System.Collections.Generic;
using System.Text;
using TeacherPlanner.Models;
using TeacherPlanner.ViewModels;

namespace TeacherPlanner.Helpers
{
    public class LoadedDays : ObservableObject
    {
        private Day _leftDay;
        private Day _rightDay;
        private string _username;
        private PageViewModel _myParent;
        private int _index = 0;
        private Day[] _pageDays;

        public LoadedDays(string username, PageViewModel parent)
        {   
            List = new Day[DAYLIMIT];
            _pageDays = new Day[2];
            _username = username;
            _myParent = parent;
            LoadNewDays();
        }

        public Day[] List;
        public const int DAYLIMIT = 14;
        public Day LeftDay
        {
            get => _pageDays[0];
            set
            {
                RaiseAndSetIfChanged(ref _pageDays[0], value);
            }
        }
        public Day RightDay
        {
            get => _pageDays[1];
            set => RaiseAndSetIfChanged(ref _pageDays[1], value);
        }
        
        private void MoveLeft()
        {
            for (int i = 1; i < List.Length; i++)
            {
                List[i - 1] = List[i];
            }
        }
        private void MoveRight()
        {
            for (int i = List.Length - 2; i > -1; i--)
            {
                List[i + 1] = List[i];
            }
        }
        private void AddDay(Day day)
        {
            List[_index] = day;
            _index++;
            if (_index >= DAYLIMIT)
            {
                _index = 0;
            }
        }
        private int DayIsLoaded(DateTime date)
        {
            for (int i = 0; i < List.Length; i++)
            {
                if (List[i] != null && List[i].NoteSection.Calendar.Date == date)
                    return i;
            }
            return -1;
        }
        private void LoadDay(Day day, int side) 
        {
            if (side == 0)
                LeftDay = day;
            else
                RightDay = day;
        }
        public void LoadDays()
        {
            DateTime[] dates = TimeTable.CurrentDates;
            for (int i = 0; i < 2; i++)
            {
                int index = DayIsLoaded(dates[i]);
                if (index != -1)
                    LoadDay(List[index], i);
                else
                    LoadNewDay(dates[i], i);
            }
        }
        private Day LoadNewDay(DateTime date, int side)
        {
            Day newDay = new Day(date, _myParent);
            newDay.Load(_username);
            AddDay(newDay);
            if (side == 0)
                LeftDay = newDay;
            else
                RightDay = newDay;
            return newDay;
        }
        private void LoadNewDays()
        {
            for (int i = 0; i < _pageDays.Length; i++)
            {
                LoadNewDay(TimeTable.CurrentDates[i], i);
            }
        }
        public void SaveDays()
        {
            for (int i = 0; i < _pageDays.Length; i++)
            {
                _pageDays[i].Save(_username);
            }
        }
    }
}
