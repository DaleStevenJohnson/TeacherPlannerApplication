using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TeacherPlanner.Login.Views
{
    /// <summary>
    /// Interaction logic for CreateAccountView.xaml
    /// </summary>
    public partial class CreateAccountView : UserControl
    {
        public CreateAccountView()
        {
            InitializeComponent();
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.BorderBrush = Brushes.Black;
            button.Background = Brushes.LightBlue;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.BorderBrush = Brushes.Transparent;
            button.Background = Brushes.LightGray;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Control box = sender as Control;
            box.Background = Brushes.White;
            //box.Background.Opacity = 0.5;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Control box = sender as Control;
            box.Background = Brushes.LightGray;
        }
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ParsePassword(((PasswordBox)sender).Password);
            }
        }
    }
}
