using System.Windows;
using TeacherPlanner.ViewModels;
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
            this.Hide();
            var Login = new AccountManagementWindow();
            Login.Show();
        }

        

    }
}
