using System;
using System.Linq;
using TeacherPlanner.Helpers;

namespace TeacherPlanner.Login.Models
{
    public class UserModel
    {
        private string _username;

        public UserModel(string username, string password)
        {
            Username = username;
            Key = GetShortHash(password, 32);
            UsernameHash = GetShortHash(username, 20);
        }
        public string Username { get; }
        public string UsernameHash { get; }
        public string Key { get; }
        private string GetShortHash(string input, int length)
        {
            length = length > 32 ? 32 : length;
            string hash = SecurePasswordHasher.Hash(input);
            int index = ((int)(char)value[0]) % 10;
            string key = hash.Substring(index, index + length);
            char[] characters = key.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }
    }
}
