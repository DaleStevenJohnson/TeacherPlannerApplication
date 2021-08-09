using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TeacherPlanner.Helpers
{
    class FileHandlingHelper
    {
        static FileHandlingHelper()
        {
            EnglishWords = File.ReadAllText(CreateFilePath(TestPath, @"EnglishWords.txt")).Split("\n");
            RandomIndexer = new Random();
        }
        private static string RootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private static string UserDataPath = CreateFilePath(RootPath, @"UserData");
        private static string TestPath = CreateFilePath(RootPath, @"Tests");
        private static string[] EnglishWords;
        private static Random RandomIndexer;

        // File Path Methods
        public static string CreateFilePath(string root, string location)
        {
            
            return Path.Combine(root, location);
        }
        private static bool AddDirectory(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return true;
        }
        private static bool FileExists(string path, string filename)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, filename);
                if (files.Length > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        // Read/write Methods
        public static string[] LoadDataFromFile(string username, string date)
        {
            string filename = date + ".txt";
            string path = CreateFilePath(UserDataPath, username);
            string yearDirectory = date.Substring(0, 4);
            path = CreateFilePath(path, yearDirectory);
            string monthDirectory = date.Substring(0, 6);
            path = CreateFilePath(path, monthDirectory);
            if (FileExists(path, filename))
            {
                path = CreateFilePath(path, filename);
                return File.ReadAllLines(path);
            }
            else
                return new string[0];
        }


        public static bool Overwrite(string username, string date, string data)
        {
            string path = CreateFilePath(UserDataPath, username);
            string yearDirectory = date.Substring(0, 4);
            path = CreateFilePath(path, yearDirectory);
            string monthDirectory = date.Substring(0, 6);
            path = CreateFilePath(path, monthDirectory);
            AddDirectory(path);
            path = CreateFilePath(path, date + ".txt");
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
        // Test Data Methods
        public static string GetRandomWord()
        {
            int index = RandomIndexer.Next(0, EnglishWords.Length);
            string word = EnglishWords[index].Trim();
            return word;
        }
        public static string GetRandomSentence(int words)
        {
            string sentence = "";
            for (int i = 0; i < words; i++)
            {
                int index = RandomIndexer.Next(0, EnglishWords.Length);
                string word = EnglishWords[index].Trim();
                sentence += word;
                if (i != words - 1) sentence += " ";
            }
            return sentence;
        }
        
    }
}
