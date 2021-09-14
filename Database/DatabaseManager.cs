using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Database.DatabaseModels;

namespace Database
{
    public class DatabaseManager
    {
        //private SQLiteConnection databaseConnection;
        private static string _connectionString = "Data Source=TeacherPlannerDB.db";
        public DatabaseManager()
        {
            //This part killed me in the beginning.  I was specifying "DataSource"
            //instead of "Data Source"
            //databaseConnection = new SQLiteConnection("Data Source=TeacherPlannerDB.db");
        }

        public static void AddUser(User user)
        {
            var query = $"INSERT INTO Users (username, password) VALUES ('{user.Username}', '{user.Password}');";
            insertQuery(query);

        }

        public static bool CheckIfUserExists(string username)
        {
            var query = $"SELECT username FROM Users WHERE username='{username}'";
            var result = selectQuery(query);
            return result.Count != 0;
        }

        public static User GetUserAccount(string username)
        {
            string query = $"SELECT username,password FROM Users WHERE username='{username}';";
            var data = selectQuery(query);
            if (data.Count == 2)
            {
                return new User()
                {
                    Username = data[0],
                    Password = data[1]
                };
            }
            return null;
        }

        public static List<User> GetUserAccounts()
        {
            string query = $"SELECT username,password FROM Users;";
            var data = selectQuery(query);
            var users = new List<User>();
            for (int i = 0; i < data.Count; i += 2)
            {
                var user = new User()
                {
                    Username = data[i],
                    Password = data[i + 1]
                };
                users.Add(user);
            }
            return users;
        }

        private static bool insertQuery(string query)
        {
            int result = 0;
            try
            {
                using (SQLiteConnection databaseConnection = new SQLiteConnection(_connectionString))
                {
                    databaseConnection.Open();  //Initiate connection to the db
                    var command = databaseConnection.CreateCommand();
                    command.CommandText = query;  //set the passed query
                    result = command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }
            return result > 0;
        }


        private static List<string> selectQuery(string query)
        {
            //SQLiteDataAdapter ad;
            //DataTable dt = new DataTable();
            List<string> results = new List<string>();
            try
            {
                using (SQLiteConnection databaseConnection = new SQLiteConnection(_connectionString))
                {
                    databaseConnection.Open();  //Initiate connection to the db
                    var command = databaseConnection.CreateCommand();
                    command.CommandText = query;  //set the passed query
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            results.Add(reader.GetString(i));
                        }
                    }
                    //ad = new SQLiteDataAdapter(command);
                    //ad.Fill(dt); //fill the datasource
                }
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }
            
            return results;
        }

        private static DataTable selectMultipleColumnsQuery(string query)
        {
            //SQLiteDataAdapter ad;
            DataTable dataTable = new DataTable();
            List<string> results = new List<string>();
            try
            {
                using (SQLiteConnection databaseConnection = new SQLiteConnection(_connectionString))
                {
                    databaseConnection.Open();  //Initiate connection to the db
                    var command = databaseConnection.CreateCommand();
                    command.CommandText = query;  //set the passed query
                    var adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(dataTable); //fill the datasource
                }
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }

            return dataTable;
        }
    }
}
