using System.Windows.Controls;
using System.Windows.Input;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.Planner.ViewModels;

namespace TeacherPlanner.Planner.Views
{
    /// <summary>
    /// Interaction logic for ChooseYearPage.xaml
    /// </summary>
    public partial class ChooseYearPage : UserControl
    {
        public ChooseYearPage()
        {
            InitializeComponent();
        }

        private void ListViewItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChooseYearViewModel viewmodel = (ChooseYearViewModel)this.DataContext;
            ListViewItem item = (ListViewItem)sender;
            YearSelectModel yearselect = (YearSelectModel)item.DataContext;
            viewmodel.OnYearSelected(yearselect.AcademicYear);
        }
    }
}
