using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TeacherPlanner.Planner.Models;

namespace TeacherPlanner.Planner.Views
{
    /// <summary>
    /// Interaction logic for PeriodView.xaml
    /// </summary>
    public partial class PeriodView : UserControl
    {
        public PeriodView()
        {
            InitializeComponent();
        }

        public void TextBoxMouseOver(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Background = Brushes.LightGray;
            textBox.BorderBrush = Brushes.Black;

        }

        public void TextBoxOutOfFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Background = Brushes.White;
            textBox.BorderBrush = Brushes.Transparent;
        }

        public void TextBoxMouseOut(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Background = Brushes.White;
            textBox.BorderBrush = Brushes.Transparent;
        }


        public void TextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            PeriodModel context = (PeriodModel)DataContext;
            context.UpdateUserEnteredClassCode(textBox.Text);
        }
    }
}
