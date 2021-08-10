using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TeacherPlanner.Helpers
{
    public class FileHandlingHelper
    {
        static FileHandlingHelper()
        {
            if (!Directory.Exists(RootPath))
                Directory.CreateDirectory(RootPath);
        }
        private static string RootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TeacherPlanner");
        private static string UserDataPath = Path.Combine(RootPath, "UserData");
        
        // File Path Methods
        private static bool AddDirectory(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return true;
        }
        // Read/write Methods
        public static string[] LoadDataFromFile(string username, string date)
        {
            string filename = date + ".txt";
            string path = Path.Combine(UserDataPath, username);
            string yearDirectory = date.Substring(0, 4);
            string monthDirectory = date.Substring(0, 6);
            path = Path.Combine(path, yearDirectory, monthDirectory, filename);
            if (File.Exists(path))
                return File.ReadAllLines(path);
            else
                return new string[0];
        }


        public static bool Overwrite(string username, string date, string data)
        {
            string path = Path.Combine(UserDataPath, username);
            string yearDirectory = date.Substring(0, 4);
            path = Path.Combine(path, yearDirectory);
            string monthDirectory = date.Substring(0, 6);
            path = Path.Combine(path, monthDirectory);
            AddDirectory(path);
            path = Path.Combine(path, date + ".txt");
            File.WriteAllText(path, data);
            return true;
        }
        // Input Sanitisation Methods
        public static string SanitiseString(string s)
        {
            return s.Trim().Replace("`", "");
        }
        public static string[] SanitiseStrings(string[] strings)
        {
            string[] cleanStrings = new string[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                cleanStrings[i] = SanitiseString(strings[i]);
            }
            return cleanStrings;
        }       
    }
}
