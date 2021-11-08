using System;
using System.Globalization;
using System.IO;
using System.Windows;

namespace TeacherPlanner.Services
{
    public class ExceptionHandlerService
    {
        public void Attach()
        {
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }
      
        private void OnDispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                var errorMessage = $"An unhandled exception occurred and the app must shut down.\nPlease see the logs at {GetLogFilePath()}";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                WriteExceptionToLogFile(e.Exception);
                e.Handled = true;
            }
            // If something goes wrong during the exception handling, we want to catch it
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }

        private static void WriteExceptionToLogFile(Exception exception)
        {
            var logPath = GetLogFilePath();
            var fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture)+".txt";
            var filePath = Path.Combine(logPath, fileName);
         
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
         
            File.WriteAllText(filePath, exception.ToString());
        }

        private static string GetLogFilePath()
        {
            // TODO: Change this to where you want the logs to go.
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataPath, "TeacherPlanner", "Logs");
        }
    }
}