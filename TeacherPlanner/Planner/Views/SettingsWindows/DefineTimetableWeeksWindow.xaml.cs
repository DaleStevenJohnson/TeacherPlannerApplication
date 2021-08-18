using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Planner.ViewModels;

namespace TeacherPlanner.Planner.Views.SettingsWindows
{
    /// <summary>
    /// Interaction logic for DefineTimetableWeeksWindow.xaml
    /// </summary>
    public partial class DefineTimetableWeeksWindow : Window
    {
        public DefineTimetableWeeksWindow(UserModel userModel)
        {
            InitializeComponent();
            var viewModel = new DefineTimetableWeeksViewModel(this, userModel);
            this.DataContext = viewModel;
        }
    }
}
