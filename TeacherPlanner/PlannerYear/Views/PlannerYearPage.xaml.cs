using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.PlannerYear.Views
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
