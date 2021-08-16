using System;
using System.IO;

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
        public static string LoggedInUserDataPath { get; set; }

        // File Path Methods
        /// <summary>
        /// Creates a directory in the format \Username\YYYY\YYYYMM
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string CreateDatedUserDirectory(string username, string date)
        {
            string path = Path.Combine(UserDataPath, username);
            string yearDirectory = date.Substring(0, 4);
            string monthDirectory = date.Substring(0, 6);
            return Path.Combine(path, yearDirectory, monthDirectory);
        }
        // Read/write Methods
        public static string[] ReadDataFromFile(string path, bool dataIsEncrypted = false, string key = "")
        {
            if (File.Exists(path))
            {
                string[] data = File.ReadAllLines(path);
                if (dataIsEncrypted)
                {
                    string dataString = Cryptography.DecryptString(key, data[0]);
                    return dataString.Split("\n");
                }
                else
                    return data;
            }
            else
                return new string[0];
        }
        public static string[][] ReadDataFromCSVFile(string path, bool dataIsEncrypted = false, string key = "")
        {
            string[] data = File.ReadAllLines(path);
            int rows = data.Length;
            string[][] csvData = new string[rows][];
            for (int i = 0; i < rows; i++)
            {
                string line = data[i];
                if (dataIsEncrypted)
                {
                    line = Cryptography.DecryptString(key, line);
                }
                string[] lineData = line.Split(",");
                csvData[i] = lineData;
            }
            return csvData;
        }
        public static bool TryWriteDataToCSVFile(string path, string filename, string[][] data, string mode = "o", bool dataShouldBeEncrypted = false, string key = "")
        {
            Directory.CreateDirectory(path);
            var filepath = Path.Combine(path, filename);
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(filepath))
            {
                for (int row = 0; row < data.Length; row++)
                {
                    string line = string.Empty;
                    for (int column = 0; column < data[row].Length; column++)
                    {
                            line += data[row][column];
                            if (column != data[row].Length - 1)
                                line += ",";
                        }
                        if (dataShouldBeEncrypted)
                        {
                            line = Cryptography.EncryptString(key, line);
                        }
                        sw.WriteLine(line);
                    }
            }
            return true;
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
                File.WriteAllText(filepath, data);
            }
            return true;
        }
        
        // Input Sanitisation Methods
        public static string SanitiseString(string s)
        {
            return s.Trim().Replace("`", string.Empty);
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
