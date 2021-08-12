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
        public static string RootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TeacherPlanner");
        public static string UserDataPath = Path.Combine(RootPath, "UserData");
        
        // File Path Methods
       
        // Read/write Methods
        public static string[] LoadDataFromFile(string path)
        {
            if (File.Exists(path))
                return File.ReadAllLines(path);
            else
                return new string[0];
        }

        public static bool TryWriteDataToFile(string path, string filename, string data, string mode = "a", bool dataShouldBeEncrypted = false, string key = "")
        {
            Directory.CreateDirectory(path);
            var filepath = Path.Combine(path, filename);
            if (dataShouldBeEncrypted)
            {
                data = Cryptography.EncryptString(key, data);
            }
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(data);
                }
            }
            else if (mode == "a")
            {
                // This text is always added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(data);
                }
            }
            else 
            {
                File.WriteAllText(path, data);
            }
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
