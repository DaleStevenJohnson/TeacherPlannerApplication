using System;
using System.Linq;
using Database.DatabaseModels;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login.Models
{
    public class UserModel
    {
        public UserModel(User user)
        {
            ID = user.ID;
            Username = user.Username;
            Key = GetEncrypted(user.Password, user.Password, 32);
        }
        public int ID { get; }
        public string Username { get; }
        public string Key { get; }
        private string GetEncrypted(string input, string keyString, int length)
        {
            string encrypted = Cryptography.EncryptString(
                    string.Concat(Enumerable.Repeat(keyString.Substring(0, 4), 8)),
                    string.Concat(Enumerable.Repeat(input.Substring(4, 4), 8))
                    );
            length = length > 32 ? 32 : length;
            if (length <= 32)
            {
                int index = ((int)(char)input[0]) % 10;
                string key = encrypted.Substring(index, length);
                char[] characters = key.ToArray();
                Array.Sort(characters);
                return new string(characters);
            }
            return encrypted;
        }
    }
}
