using System;
using System.IO;
using TeacherPlanner.Constants;
using TeacherPlanner.Login.Models;

namespace TeacherPlanner.Helpers
{
    public class FileHandlingHelper
    {
        private const bool ENCRYPTING_DIRECTORIES = false; 
        static FileHandlingHelper()
        {
            if (!Directory.Exists(RootPath))
                Directory.CreateDirectory(RootPath);
        }
        public static string Secret = "password12345678password12345678";
        public static string RootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TeacherPlanner");
        public static string UserDataPath = Path.Combine(RootPath, EncryptFileOrDirectory("userdata"));
        public static string ApplicationConfigPath = Path.Combine(RootPath, EncryptFileOrDirectory("config"));
        public static string LoggedInUserPath { get; set; }
        public static string LoggedInUserDataPath { get; set; }
        public static string LoggedInUserConfigPath { get; set; }
        public static void SetDirectories(UserModel userModel, string year = "")
        {
            //TODO - Encrypt all directory names (or just port it all to a SQL DB...)
            var username = EncryptFileOrDirectory(userModel.Username, userModel.Key);
            LoggedInUserPath = Path.Combine(UserDataPath, username);
            if (year != string.Empty)
            {
                year = EncryptFileOrDirectory(year, userModel.Key);
                LoggedInUserDataPath = Path.Combine(UserDataPath, username, year);
                LoggedInUserConfigPath = Path.Combine(LoggedInUserDataPath, EncryptFileOrDirectory("config", userModel.Key));
            }
        }
        
        public static string CreateMonthlyUserDataDirectory(string date, string key)
        {
            string monthDirectory = EncryptFileOrDirectory(date.Substring(0, 6), key);
            return Path.Combine(LoggedInUserDataPath, monthDirectory);
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

        // Encryption Methods

        /// <summary>
        /// Takes a given directory name or filename and safely encrypts it.
        /// </summary>
        /// <param name="name">Name of the file or directory you wish to encrypt</param>
        /// <param name="userkey">User provided key for encryption. If not provided, will use system defined key</param>
        /// <returns>Encrypted string which is safe to be used as a filename or directory</returns>
        public static string EncryptFileOrDirectory(string name, string userkey = "")
        {
            var key = userkey == "" ? Secret : userkey;
            var IsFilename = name.Contains(".");
            var plaintext = IsFilename ? name.Split(".")[0] : name;
            
            // Encrypting Directories is a private const used as a global switch for
            // turning off and on encryption of directories and filenames.
            // As it is a const, the IDE complains a bit about unreachable code. Please ignore that for now.
            if (ENCRYPTING_DIRECTORIES)
            {
                var ciphertext = Cryptography.EncryptString(key, plaintext);
                ciphertext = ciphertext.Replace(@"\", string.Empty);
                ciphertext = ciphertext.Replace(@"/", string.Empty);
                ciphertext = IsFilename ? ciphertext + name.Split(".")[1] : ciphertext;
                return ciphertext;
            }
            return name;
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
