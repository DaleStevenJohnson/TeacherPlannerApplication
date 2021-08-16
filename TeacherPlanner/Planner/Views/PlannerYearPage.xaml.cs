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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeacherPlanner.Planner.Views
{
    /// <summary>
    /// Interaction logic for PlannerYearPage.xaml
    /// </summary>
    public partial class PlannerYearPage : UserControl
    {
        public PlannerYearPage()
        {
            InitializeComponent();
        }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
