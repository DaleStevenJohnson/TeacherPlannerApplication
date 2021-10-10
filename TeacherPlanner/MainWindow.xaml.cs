using System.Windows;
using TeacherPlanner.Login.Models;
using TeacherPlanner.Login.ViewModels;
using TeacherPlanner.Login.Views;

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

            var user = new UserModel(accountManagementViewModel.LoginViewModel.User);
            var mainViewModel = new MainViewModel(user);

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
