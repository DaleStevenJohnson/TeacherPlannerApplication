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
            var parameters = new Dictionary<string, object> { { "@UserID", userID }, { "@Year", year } };
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


        //
        // Todo List
        //

        public static bool TrySaveTodoList(TodoList todoList, out int id)
        {
            var query = "INSERT INTO TodoLists (academic_year_id, name) VALUES (@AcademicYearID, @Name);";
            var result = InsertModelsIntoDatabase(query, todoList);
            id = GetLastIDFromDatabase("TodoLists");
            return result;
        }

        public static List<TodoList> GetTodoLists(int academicYearID)
        {
            string query = "SELECT * FROM TodoLists WHERE academic_year_id=@ID;";
            var parameters = new Dictionary<string, object> { { "@ID", academicYearID } };
            return GetModelsFromDatabase<TodoList>(query, parameters);
        }

        public static bool RemoveTodoList(int id)
        {
            RemoveAllTodoListItems(id);
            var query = $"DELETE FROM TodoLists WHERE id=@ID";
            var parameters = new Dictionary<string, object> { { "@ID", id } };
            return RemoveFromDatabase(query, parameters);
        }

        public static bool TryUpdateTodoList(TodoList todoList)
        {
            var query = "UPDATE TodoLists SET name=@Name WHERE id=@ID;";
            return UpdateModelsInDatabase(query, todoList);
        }

        //
        // Todo Items
        //

        public static bool RemoveAllTodoListItems(int id)
        {
            var query = $"DELETE FROM TodoItems WHERE todo_list_id=@ID";
            var parameters = new Dictionary<string, object> { { "@ID", id } };
            return RemoveFromDatabase(query, parameters);
        }

        public static List<TodoItem> GetTodoItems(int listID)
        {
            string query = "SELECT * FROM TodoItems WHERE todo_list_id=@ID;";
            var parameters = new Dictionary<string, object> { { "@ID", listID } };
            return GetModelsFromDatabase<TodoItem>(query, parameters);
        }

        public static bool RemoveTodoItem(int id)
        {
            var query = $"DELETE FROM TodoItems WHERE id=@ID";
            var parameters = new Dictionary<string, object> { { "@ID", id } };
            return RemoveFromDatabase(query, parameters);
        }


        public static bool TrySaveTodoItem(TodoItem todoItem, out int id)
        {
            var query = "INSERT INTO TodoItems (todo_list_id, is_sub_item, parent_item, description, is_completed) VALUES (@TodoListID,  @IsSubItem, @ParentItem, @Description, @IsCompleted);";
            var result = InsertModelsIntoDatabase(query, todoItem);
            id = GetLastIDFromDatabase("TodoItems");
            return result;
        }

        public static bool TryUpdateTodoItem(TodoItem todoItem)
        {
            var query = "UPDATE TodoItems SET description=@Description, is_completed=@IsCompleted WHERE id=@ID;";
            return UpdateModelsInDatabase(query, todoItem);
        }




        // Private Methods

        private static int GetLastIDFromDatabase(string tablename)
        {
            if (CheckTableExists(tablename))
            {
                var query = $"SELECT id FROM {tablename} WHERE id=(SELECT max(id) FROM {tablename});";
                using (IDbConnection connection = new SQLiteConnection(_connectionString))
                {
                    return connection.QuerySingle<int>(query, new DynamicParameters());
                }
            }
            return -1;
        }

        private static bool CheckTableExists(string tableName)
        {
            string query = "SELECT COUNT(name) FROM sqlite_master WHERE name=@tableName;";
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                return connection.QuerySingle<int>(query, new { tableName }) == 1;
            }
        }

        private static bool RemoveFromDatabase(string query, Dictionary<string, object> parameters)
        {
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                return connection.Execute(query, parameters) > 0;
            }
        }

        private static bool UpdateModelsInDatabase(string query, params object[] models)
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
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                var output = connection.Query<T>(query, new DynamicParameters(parameters));
                return output.AsList();
            }
        }
    }
}
