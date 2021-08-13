using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TeacherPlanner.ToDo.Views
{
    /// <summary>
    /// Interaction logic for TodoPageView.xaml
    /// </summary>
    public partial class TodoPageView : UserControl
    {
        public TodoPageView()
        {
            InitializeComponent();
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.BorderBrush = new SolidColorBrush(Colors.Black) { Opacity = 0.2 };
            button.BorderThickness = new Thickness(0.5);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.BorderBrush = Brushes.Transparent;
            button.BorderThickness = new Thickness(0);
        }
    }
}
