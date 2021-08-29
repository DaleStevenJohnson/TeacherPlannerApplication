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
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Planner.Views
{
    /// <summary>
    /// Interaction logic for PlannerYearPage.xaml
    /// </summary>
    public partial class PlannerYearPage : UserControl
    {
        private ICommand LoseFocusCommand;
        public PlannerYearPage()
        {
            InitializeComponent();
            LoseFocusCommand = new SimpleCommand(_ => PlannerYearWindow_MouseDown());
            KeyBinding LoseFocusBinding = new KeyBinding(
                LoseFocusCommand,
                Key.Escape, ModifierKeys.None);

            this.InputBindings.Add(LoseFocusBinding);
        }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PlannerYearWindow_MouseDown(object sender = null, MouseButtonEventArgs e = null)
        {
            FocusManager.SetFocusedElement(this, null);
            Keyboard.ClearFocus();
        }
    }
}
