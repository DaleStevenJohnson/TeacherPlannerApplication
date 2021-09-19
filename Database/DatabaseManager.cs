using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Database.DatabaseModels;

namespace Database
{
    public class DatabaseManager
    {
        //private SQLiteConnection databaseConnection;
        private static string _connectionString = "Data Source=TeacherPlannerDB.db";
       
        
        //
        // User
        //

        public static bool TryAddUser(User user)
        {
            var query = "INSERT INTO Users (username, password) VALUES (@Username, @Password);";
            return InsertModelsIntoDatabase(query, user);
        }

        public static bool CheckIfUserExists(string username)
        {
            var query = "SELECT username FROM Users WHERE username=@Username";
            var parameters = new Dictionary<string, object> { { "@Username", username } };
            var result = GetModelsFromDatabase<User>(query, parameters);
            return result.Count != 0;
        }

        public static User GetUserAccount(string username)
        {
            string query = "SELECT * FROM Users WHERE username=@Username;";
            var parameters = new Dictionary<string, object> { { "@Username", username } };
            var data = GetModelsFromDatabase<User>(query, parameters);
            if (data.Count == 1)
            {
                return data[0];
            }
            return null;
        }

        public static List<User> GetUserAccounts()
        {
            string query = "SELECT * FROM Users;";
            return GetModelsFromDatabase<User>(query);
        }

        
        //
        // Academic Years
        //

        public static List<AcademicYear> GetAcademicYears(int ID)
        {
            string query = "SELECT * FROM AcademicYears WHERE user_id=@ID;";
            var parameters = new Dictionary<string, object> { { "@ID", ID } };
            return GetModelsFromDatabase<AcademicYear>(query, parameters);
        }

        public static bool TrySaveAcademicYear(AcademicYear academicYear)
        {
            var query = "INSERT INTO AcademicYears (year, user_id) VALUES (@Year, @UserID);";
            return InsertModelsIntoDatabase(query, academicYear);
        }

        public static bool CheckIfAcademicYearExists(int userID, int year)
        {
            var query = "SELECT year FROM AcademicYears WHERE user_id=@UserID AND year=@Year";
            var parameters = new Dictionary<string, object> { { "@UserID", userID }, { "@Year", year} };
            var result = GetModelsFromDatabase<AcademicYear>(query, parameters);
            return result.Count != 0;
        }


        //
        // Timetable Weeks
        //

        public static List<TimetableWeek> GetTimetableWeeks(int academicYearID)
        {
            string query = "SELECT * FROM TimetableWeeks WHERE academic_year_id=@ID;";
            var parameters = new Dictionary<string, object> { { "@ID", academicYearID } };
            return GetModelsFromDatabase<TimetableWeek>(query, parameters);
        }


        // Private Methods

        private static bool InsertModelsIntoDatabase(string query, params object[] models)
        {
            int rowsAffected = 0;
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                foreach (object model in models)
                {
                    rowsAffected += connection.Execute(query, model);
                }
            }
            return rowsAffected > 0;
        }

        private static List<T> GetModelsFromDatabase<T>(string query, Dictionary<string, object> parameters = null)
        {
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                var output = connection.Query<T>(query, new DynamicParameters(parameters));
                return output.ToList();
            }
        }
    }
}
