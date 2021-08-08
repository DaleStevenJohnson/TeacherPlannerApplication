using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeacherPlanner.Views
{
    /// <summary>
    /// Interaction logic for TextBoxEntryView.xaml
    /// </summary>
    public partial class RowEntryView : UserControl
    {
        public RowEntryView()
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
            //textBox.Background = Brushes.White;
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
