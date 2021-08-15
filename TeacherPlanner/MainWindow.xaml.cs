using System.Windows;
using TeacherPlanner.Login.ViewModels;
using TeacherPlanner.Login.Views;
using TeacherPlanner.Planner.ViewModels;
using TeacherPlanner.Planner.Views.SettingsWindows;

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
            DataContext = new MainViewModel(accountManagementViewModel.LoginViewModel.UserModel);
            DataContextChanged += (sender, args) =>
            {
                DataContext = args.NewValue;
            };           
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
