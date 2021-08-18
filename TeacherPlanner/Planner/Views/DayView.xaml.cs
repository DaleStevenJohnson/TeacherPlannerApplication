using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TeacherPlanner.Planner.Views
{
    /// <summary>
    /// Interaction logic for DayView.xaml
    /// </summary>
    public partial class DayView : UserControl
    {
        public DayView()
        {
            InitializeComponent();
        }

        private void NextArrow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = button.Background = (ImageBrush)Application.Current.FindResource("Icon.NextArrow.Hovered");
        }

        private void NextArrow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = button.Background = (ImageBrush)Application.Current.FindResource("Icon.NextArrow.Normal");
        }
        private void PreviousArrow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = button.Background = (ImageBrush)Application.Current.FindResource("Icon.PreviousArrow.Hovered");
        }

        private void PreviousArrow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = button.Background = (ImageBrush)Application.Current.FindResource("Icon.PreviousArrow.Normal");
        }
    }
}
