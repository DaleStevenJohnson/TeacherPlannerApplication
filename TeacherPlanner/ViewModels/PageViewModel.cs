using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Models;

namespace TeacherPlanner.ViewModels
{
    public class PageViewModel : ObservableObject
    {
        private Day _leftDay;
        private Day _rightDay;
        private TextBlock _debug;

        public PageViewModel(TextBlock DEBUGGER)
        {
            LeftDay = new Day(TimeTable.CurrentDateLeft, this);
            RightDay = new Day(TimeTable.CurrentDateRight, this);
            _debug = DEBUGGER;
            TurnPageForwardCommand = new SimpleCommand(numOfDays => OnTurnPageForward(Convert.ToInt32(numOfDays)));
            TurnPageBackwardCommand = new SimpleCommand(numOfDays => OnTurnPageBackward(Convert.ToInt32(numOfDays)));
        }

        public ICommand TurnPageForwardCommand { get; }
        public ICommand TurnPageBackwardCommand { get; }

        public Day LeftDay
        {
            get => _leftDay;
            set => RaiseAndSetIfChanged(ref _leftDay, value);
        }

        public Day RightDay
        {
            get => _rightDay;
            set => RaiseAndSetIfChanged(ref _rightDay, value);
        }

        public void OnTurnPageForward(int numOfDays)
        {
            TimeTable.ChangeCurrentDate(numOfDays, "Left");
            LeftDay = new Day(TimeTable.CurrentDateLeft, this);
            RightDay = new Day(TimeTable.CurrentDateRight, this);
            _debug.Text = $"{LeftDay.Period1.Row1.LeftText}";
        }

        public void OnTurnPageBackward(int numOfDays)
        {
            TimeTable.ChangeCurrentDate(numOfDays, "Left");
            LeftDay = new Day(TimeTable.CurrentDateLeft, this);
            RightDay = new Day(TimeTable.CurrentDateRight, this);
            Random r = new Random();
            _debug.Text = $"{r.Next(1, 10)}:{LeftDay.Period1.Row1.LeftText}";
        }

    }

   
}
