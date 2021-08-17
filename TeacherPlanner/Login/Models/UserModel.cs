using System;
using System.Linq;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login.Models
{
    public class UserModel
    {
        public UserModel(string username, string password)
        {
            Username = username;
            Key = GetEncrypted(password, password, 32);
            UsernameHash = GetEncrypted(username, password, 20);
        }
        public string Username { get; }
        public string UsernameHash { get; }
        public string Key { get; }
        private string GetEncrypted(string input, string keyString, int length)
        {
            string encrypted = Cryptography.EncryptString(
                    string.Concat(Enumerable.Repeat(keyString.Substring(0, 4), 8)),
                    string.Concat(Enumerable.Repeat(input.Substring(4, 4), 8))
                    );
            length = length > 32 ? 32 : length;
            if (length < 32)
            {
                int index = ((int)(char)input[0]) % 10;
                string key = encrypted.Substring(index, index + length);
                char[] characters = key.ToArray();
                Array.Sort(characters);
                return new string(characters);
            }
            return encrypted;
        }
    }
}
