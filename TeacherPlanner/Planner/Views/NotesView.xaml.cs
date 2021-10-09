using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TeacherPlanner.Planner.Views
{
    /// <summary>
    /// Interaction logic for NotesView.xaml
    /// </summary>
    public partial class NotesView : UserControl
    {
        public NotesView()
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
            TextBox textBox = sender as TextBox;
        }
    }
}
