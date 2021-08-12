using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeacherPlanner.Helpers;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Models;

namespace TeacherPlanner.ViewModels
{
    
    public class PlannerViewModel
    {
        public PlannerViewModel(UserModel userModel)
        {
            UserModel = userModel;
            Days = new LoadedDays(UserModel.Username, this);
            Days.LoadDays();
            TurnPageForwardCommand = new SimpleCommand(numOfDays => OnTurnPageForward(Convert.ToInt32(numOfDays)));
            TurnPageBackwardCommand = new SimpleCommand(numOfDays => OnTurnPageBackward(Convert.ToInt32(numOfDays)));
            SaveCommand = new SimpleCommand(_ => OnSave());
        }
        public UserModel UserModel;

        public ICommand TurnPageForwardCommand { get; }
        public ICommand TurnPageBackwardCommand { get; }
        public ICommand SaveCommand { get; }

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
        public void OnSave()
        {
            Days.SaveDays();
            MessageBox.Show("Saved");
        }


    }


}
