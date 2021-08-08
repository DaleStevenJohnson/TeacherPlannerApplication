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
        private int _marginTextColumn;
        private int _mainTextColumn;

        public MainWindow()
        {

            InitializeComponent();
            
            MarginTextColumn = 2;

            PageViewModel = new PageViewModel(DEBUGGER);
            
            this.DataContext = PageViewModel;

            DEBUGGER.Text = $"{PageViewModel.LeftDay.Periods[0].MarginFields[0]}";

            //Day1.Text = DataHandling.Time.Today;
            //Day2.Text = DataHandling.Time.Tomorrow;
            //Grid root = Layout_Root;

            //// Add 6 periods for both days
            //int periods = 6;
            //for (int day = 1; day <= 3; day += 2)
            //{
            //    for (int period = 1; period <= periods; period++)
            //    {
            //        Grid newPeriod = CreatePeriodBlock(period, day);
            //        Grid.SetColumn(newPeriod, day);
            //        Grid.SetRow(newPeriod, period + 1);
            //        root.Children.Add(newPeriod);
            //    }

            //    Grid newNotesBlock = CreateNotesSection(day);
            //    Grid.SetColumn(newNotesBlock, day);
            //    Grid.SetRow(newNotesBlock, 8);
            //    root.Children.Add(newNotesBlock);
            //}
        }

        public PageViewModel PageViewModel { get; set; }

        public int MarginTextColumn {
            get 
            {
                return _marginTextColumn;
            }
            set 
            {
                _marginTextColumn = value;
                MainTextColumn = _marginTextColumn + 1;
            }
        }
        public int MainTextColumn
        {
            get
            {
                return _mainTextColumn;
            }
            set
            {
                _mainTextColumn = value;
                SideTextColumn = _mainTextColumn + 1;
            }
        }
        public int SideTextColumn { get; set; }

        private Grid CreateNotesSection(int day)
        {
            int[] columnSizes = new int[] { 1, 18, 3, 1 };
            int[] rowSizes = new int[] { 1, 1, 1, 1, 1, 1, 1, 2 };

            Grid container = new Grid();



            Grid notesBlock = new Grid();


            Border border = CreateBorder(2, 2, 2, 2);
            Grid.SetColumn(border, 1);
            container.Children.Add(border);

            border.Child = notesBlock;

            // Create Column Definitions
            for (int i = 0; i < columnSizes.Length; i++)
            {
                ColumnDefinition column_definition = new ColumnDefinition();
                column_definition.Width = new GridLength(columnSizes[i], GridUnitType.Star);
                notesBlock.ColumnDefinitions.Add(column_definition);
            }

            // Create Row Definitions
            for (int i = 0; i < rowSizes.Length; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(rowSizes[i], GridUnitType.Star);
                notesBlock.RowDefinitions.Add(rowDefinition);
            }

            Border notes_heading_border = CreateBorder(0, 0, 0, 1, 0, 1);
            TextBlock notes_heading = CreateTextBlock("Other Activities / Personal Notes", 0, 1, 10, Brushes.White, Brushes.Black);
            notes_heading.TextAlignment = TextAlignment.Left;
            notes_heading.FontWeight = FontWeights.Bold;
            notes_heading.Margin = new Thickness(0, 5, 0, 0);
            notes_heading_border.Child = notes_heading;
            notesBlock.Children.Add(notes_heading_border);

            for (int i = 1; i < rowSizes.Length; i++)
            {
                int border_thickness = i < rowSizes.Length - 1 ? 1 : 0;

                Border notes_box_border = CreateBorder(0, 0, 0, border_thickness, i, 1);
                TextBox notes_box = CreateUserInputBox($"Notes{i + 1}_Day{day}", i, 1);
                notes_box_border.Child = notes_box;
                notesBlock.Children.Add(notes_box_border);
            }

            return container;
        }

        private Grid CreatePeriodBlock(int period, int day)
        {
            int[] columnSizes = new int[] { 2, 1, 15, 3 };
            int[] rowSizes = new int[] { 1, 1, 1, 1, 1, 1, 1 };
            Grid container = new Grid();
            Grid periodBlock = new Grid();

            Border border = CreateBorder(0, 2, 0, 0);

            container.Children.Add(border);

            border.Child = periodBlock;

            // Create Column Definitions
            for (int i = 0; i < columnSizes.Length; i++)
            {
                ColumnDefinition column_definition = new ColumnDefinition();
                column_definition.Width = new GridLength(columnSizes[i], GridUnitType.Star);
                periodBlock.ColumnDefinitions.Add(column_definition);
            }

            // Create Row Definitions
            for (int i = 0; i < rowSizes.Length; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(rowSizes[i], GridUnitType.Star);
                periodBlock.RowDefinitions.Add(rowDefinition);
            }

            TextBlock periodNumberBox = CreateTextBlock(period.ToString(), 0, 0, 60, (SolidColorBrush)new BrushConverter().ConvertFrom("#AAAAAA"), Brushes.Black);
            Grid.SetRowSpan(periodNumberBox, 5);
            periodBlock.Children.Add(periodNumberBox);

            TextBlock formGroupLabel = CreateTextBlock("Class", 5, 0, 10, Brushes.White, Brushes.Black);
            periodBlock.Children.Add(formGroupLabel);

            TextBox form_group = CreateUserInputBox($"FormGroup_Period{period}_Day{day}", 6, 0);
            form_group.TextAlignment = TextAlignment.Center;
            periodBlock.Children.Add(form_group);

            for (int i = 0; i < rowSizes.Length; i++)
            {
                int border_thickness = i < rowSizes.Length - 1 ? 1 : 0;

                Border margin_box_border = CreateBorder(1, 0, 1, border_thickness, i, 1);
                TextBox margin_box = CreateUserInputBox($"Margin{i + 1}_Period{period}_Day{day}", i, 1);
                margin_box_border.Child = margin_box;
                periodBlock.Children.Add(margin_box_border);

                Border main_text_box_border = CreateBorder(0, 0, 1, border_thickness, i, 2);
                TextBox main_text_box = CreateUserInputBox($"MainText{i + 1}_Period{period}_Day{day}", i, 2);
                main_text_box_border.Child = main_text_box;
                periodBlock.Children.Add(main_text_box_border);

                Border side_text_box_border = CreateBorder(0, 0, 1, border_thickness, i, 3);
                TextBox side_text_box = CreateUserInputBox($"SideText{i + 1}_Period{period}_Day{day}", i, 3);
                side_text_box_border.Child = side_text_box;
                periodBlock.Children.Add(side_text_box_border);
            }

            return container;
        }

        private Border CreateBorder(double left, double top, double right, double bottom, int row = 0, int column = 0)
        {
            Border border = new Border();
            border.BorderBrush = Brushes.Gray;
            border.BorderThickness = new Thickness(left, top, right, bottom);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
            return border;
        }

        private TextBox CreateUserInputBox(string name, int row, int column)
        {
            TextBox user_input_box = new TextBox();
            user_input_box.Name = name;
            user_input_box.MouseEnter += TextBoxMouseOver;
            user_input_box.MouseLeave += TextBoxMouseOut;
            user_input_box.LostFocus += TextBoxOutOfFocus;
            user_input_box.SelectionChanged += TextBoxSelectionChanged;
            user_input_box.AcceptsReturn = true;
            user_input_box.TextWrapping = System.Windows.TextWrapping.Wrap;
            user_input_box.IsReadOnly = true;
            user_input_box.BorderBrush = Brushes.Transparent;

            Grid.SetRow(user_input_box, row);
            Grid.SetColumn(user_input_box, column);
            return user_input_box;
        }

        private TextBlock CreateTextBlock(string content, int row, int column, int font_size, SolidColorBrush background_colour, SolidColorBrush foreground_colour)
        {
            // Create Period Number
            TextBlock period_number_box = new TextBlock();
            period_number_box.Text = content;
            period_number_box.TextAlignment = TextAlignment.Center;
            period_number_box.Background = background_colour;
            period_number_box.FontSize = font_size;
            period_number_box.Foreground = foreground_colour;

            Grid.SetRow(period_number_box, row);
            Grid.SetColumn(period_number_box, column);


            return period_number_box;
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
            DEBUGGER.Text = $"{PageViewModel.LeftDay.Periods[0].MarginFields[0]}";
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
