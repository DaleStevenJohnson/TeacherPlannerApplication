using System.Windows;
using TeacherPlanner.Login.ViewModels;
using TeacherPlanner.Login.Views;
using TeacherPlanner.Planner.ViewModels;
using TeacherPlanner.Planner.Views.SettingsWindows;
using TeacherPlanner.Planner.Views;
using System.Windows.Controls;
using TeacherPlanner.Login.Models;

namespace TeacherPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var accountManagementViewModel = new AccountManagementViewModel();
            var accountManagementWindow = new AccountManagementWindow
            {
                DataContext = accountManagementViewModel,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            accountManagementWindow.ShowDialog();
            if (!accountManagementViewModel.LoginViewModel.LoggedIn)
            {
                Application.Current.Shutdown();
            }
            // Todo change how UserModel is being created
            var mainViewModel = new MainViewModel(accountManagementViewModel.LoginViewModel.UserModel);
            this.DataContext = mainViewModel;
            this.DataContextChanged += (sender, args) =>
            {
                this.DataContext = args.NewValue;
            };

            
            //var chooseYearViewModel = new ChooseYearViewModel();
            //UserControl chooseYearPage = new ChooseYearPage() { DataContext = chooseYearViewModel};
            //this.AddLogicalChild(chooseYearPage);
        }

        
    }
}
