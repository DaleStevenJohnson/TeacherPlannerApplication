using System.Windows;
using TeacherPlanner.Services;

namespace TeacherPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ExceptionHandlerService _exceptionHandlerService;
        public App()
        {
            _exceptionHandlerService = new ExceptionHandlerService();
            _exceptionHandlerService.Attach();
        }
    }
}
