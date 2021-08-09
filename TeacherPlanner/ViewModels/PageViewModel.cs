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
    
    public class PageViewModel
    {
        
        private TextBlock _debug;
        private string _username = "DJohnson";

        public PageViewModel(TextBlock DEBUGGER)
        {
            Days = new LoadedDays(_username, this);
            Days.LoadDays();
            _debug = DEBUGGER;
            TurnPageForwardCommand = new SimpleCommand(numOfDays => OnTurnPageForward(Convert.ToInt32(numOfDays)));
            TurnPageBackwardCommand = new SimpleCommand(numOfDays => OnTurnPageBackward(Convert.ToInt32(numOfDays)));
        }


        public ICommand TurnPageForwardCommand { get; }
        public ICommand TurnPageBackwardCommand { get; }

        public LoadedDays Days;

        public void OnTurnPageForward(int numOfDays)
        {
            Days.SaveDays();
            TimeTable.ChangeCurrentDate(numOfDays, "Right");
            Days.LoadDays();
            //_debug.Text = $"{LeftDay.Period1.Row1.LeftText}";
        }

        public void OnTurnPageBackward(int numOfDays)
        {
            Days.SaveDays();
            TimeTable.ChangeCurrentDate(numOfDays, "Left");
            Days.LoadDays();
            
            //_debug.Text = $"{r.Next(1, 10)}:{LeftDay.Period1.Row1.LeftText}";
        }



    }


}
