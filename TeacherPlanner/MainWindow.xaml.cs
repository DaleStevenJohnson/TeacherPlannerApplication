using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using TeacherPlanner.ViewModels;

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
            
  

            PageViewModel = new PageViewModel(DEBUGGER);
            
            this.DataContext = PageViewModel;

            DEBUGGER.Text = $"{PageViewModel.LeftDay.Period1.Row1.LeftText}";

        }

        public PageViewModel PageViewModel { get; set; }

        

        // ToDo: Add XAML for the below then delete
        //private Grid CreateNotesSection(int day)
        //{
        //    int[] columnSizes = new int[] { 1, 18, 3, 1 };
        //    int[] rowSizes = new int[] { 1, 1, 1, 1, 1, 1, 1, 2 };

        //    Grid container = new Grid();



        //    Grid notesBlock = new Grid();


        //    Border border = CreateBorder(2, 2, 2, 2);
        //    Grid.SetColumn(border, 1);
        //    container.Children.Add(border);

        //    border.Child = notesBlock;

        //    // Create Column Definitions
        //    for (int i = 0; i < columnSizes.Length; i++)
        //    {
        //        ColumnDefinition column_definition = new ColumnDefinition();
        //        column_definition.Width = new GridLength(columnSizes[i], GridUnitType.Star);
        //        notesBlock.ColumnDefinitions.Add(column_definition);
        //    }

        //    // Create Row Definitions
        //    for (int i = 0; i < rowSizes.Length; i++)
        //    {
        //        RowDefinition rowDefinition = new RowDefinition();
        //        rowDefinition.Height = new GridLength(rowSizes[i], GridUnitType.Star);
        //        notesBlock.RowDefinitions.Add(rowDefinition);
        //    }

        //    Border notes_heading_border = CreateBorder(0, 0, 0, 1, 0, 1);
        //    TextBlock notes_heading = CreateTextBlock("Other Activities / Personal Notes", 0, 1, 10, Brushes.White, Brushes.Black);
        //    notes_heading.TextAlignment = TextAlignment.Left;
        //    notes_heading.FontWeight = FontWeights.Bold;
        //    notes_heading.Margin = new Thickness(0, 5, 0, 0);
        //    notes_heading_border.Child = notes_heading;
        //    notesBlock.Children.Add(notes_heading_border);

        //    for (int i = 1; i < rowSizes.Length; i++)
        //    {
        //        int border_thickness = i < rowSizes.Length - 1 ? 1 : 0;

        //        Border notes_box_border = CreateBorder(0, 0, 0, border_thickness, i, 1);
        //        TextBox notes_box = CreateUserInputBox($"Notes{i + 1}_Day{day}", i, 1);
        //        notes_box_border.Child = notes_box;
        //        notesBlock.Children.Add(notes_box_border);
        //    }

        //    return container;
        //}







    }
}
