using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            textBox.IsReadOnly = false;
            textBox.BorderBrush = Brushes.Black;

        }

        public void TextBoxOutOfFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Background = Brushes.White;
            //DEBUGGER.Text = $"{PageViewModel.LeftDay.Periods[0].MarginFields[0]}";
            textBox.BorderBrush = Brushes.Transparent;
            textBox.IsReadOnly = true;
        }

        public void TextBoxMouseOut(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Background = Brushes.White;
            textBox.BorderBrush = Brushes.Transparent;
        }


        public void TextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            //DEBUGGER.Text = $"{textBox.Name}";
        }

    }
}
