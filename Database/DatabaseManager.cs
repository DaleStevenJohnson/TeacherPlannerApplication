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

        public static bool TryAddAcademicYear(AcademicYear academicYear)
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
        // Days
        //

        public Day GetDay(int academic_year_id, DateTime date)
        {
            string query = $"SELECT * FROM Days WHERE academic_year_id=@ID AND date=@Date;";
            var parameters = new Dictionary<string, object> { { "@ID", academic_year_id }, { "@Date", date} };
            var models = GetModelsFromDatabase<Day>(query, parameters);
            
            return models.Any() == true ? models.First() : null;
        }

        public static bool TryAddDay(Day day, out int id)
        {
            var query = "INSERT INTO Days (academic_year_id, date, notes) VALUES (@AcademicYearID,  @Date, @Notes);";
            var result = InsertModelsIntoDatabase(query, day);
            id = GetLastIDFromDatabase("Days");
            return result;
        }

        public static bool TryUpdateDay(Day day)
        {
            var query = "UPDATE Days SET notes=@Notes WHERE id=@ID;";
            return UpdateModelsInDatabase(query, day);
        }

        //
        // Periods
        //
        public List<Period> GetPeriods(int day_id)
        {
            string query = $"SELECT * FROM Periods WHERE day_id=@ID;";
            var parameters = new Dictionary<string, object> { { "@ID", day_id} };
            return GetModelsFromDatabase<Period>(query, parameters);
        }

        public static bool TryAddPeriod(Period period, out int id)
        {
            var query = "INSERT INTO Periods (day_id, timetable_classcode, user_entered_classcode, period_number, margin_text, main_text, side_text) VALUES (@DayID,  @TimetableClasscode, @UserEnteredClasscode, @PeriodNumber, @MarginText, @MainText, @SideText);";
            var result = InsertModelsIntoDatabase(query, period);
            id = GetLastIDFromDatabase("Periods");
            return result;
        }

        public static bool TryUpdatePeriod(Period period)
        {
            var query = "UPDATE Periods SET timetable_classcode=@TimetableClasscode, user_entered_classcode=@UserEnteredClasscode, period_number=@PeriodNumber, margin_text=@MarginText, main_text=@MainText, side_text=@SideText WHERE id=@ID;";
            return UpdateModelsInDatabase(query, period);
        }

        //
        // Timetable Weeks
        //

        public static List<TimetableWeek> GetTimetableWeeks(int academicYearID)
        {
            return GetModelsFromDatabaseByAcademicYear<TimetableWeek>("TimetableWeeks", academicYearID);
        }

        public static bool TryAddTimetableWeek(TimetableWeek timetableWeek, out int id)
        {
            var query = "INSERT INTO TimetableWeeks (academic_year_id, week_beginning, week) VALUES (@AcademicYearID,  @WeekBeginning, @Week);";
            var result = InsertModelsIntoDatabase(query, timetableWeek);
            id = GetLastIDFromDatabase("TimetableWeeks");
            return result;
        }

        public static bool TryUpdateTimetableWeek(TimetableWeek timetableWeek)
        {
            var query = "UPDATE TimetableWeeks SET week=@Week WHERE id=@ID;";
            return UpdateModelsInDatabase(query, timetableWeek);
        }

        //
        // Keydates
        //

        public static List<KeyDate> GetKeyDates(int academicYearID)
        {
            return GetModelsFromDatabaseByAcademicYear<KeyDate>("KeyDates", academicYearID);
        }

        public static bool TryAddKeyDate(KeyDate keyDate, out int id)
        {
            var query = "INSERT INTO KeyDates (academic_year_id, description, type, datetime) VALUES (@AcademicYearID, @Description, @Type, @DateTime);";
            var result = InsertModelsIntoDatabase(query, keyDate);
            id = GetLastIDFromDatabase("KeyDates");
            return result;
        }

        public static bool TryRemoveKeyDate(int id)
        {
            return RemoveFromDatabaseByID("KeyDates", id);
        }


        //
        // Todo List
        //

        public static bool TryAddTodoList(TodoList todoList, out int id)
        {
            var query = "INSERT INTO TodoLists (academic_year_id, name) VALUES (@AcademicYearID, @Name);";
            var result = InsertModelsIntoDatabase(query, todoList);
            id = GetLastIDFromDatabase("TodoLists");
            return result;
        }

        public static List<TodoList> GetTodoLists(int academicYearID)
        {
            return GetModelsFromDatabaseByAcademicYear<TodoList>("TodoLists", academicYearID);
        }

        public static bool RemoveTodoList(int id)
        {
            RemoveAllTodoListItems(id);
            return RemoveFromDatabaseByID("TodoLists", id);
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
            return RemoveFromDatabaseByID("TodoItems", id);
        }


        public static bool TryAddTodoItem(TodoItem todoItem, out int id)
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

        //
        // Timetable Periods
        //

        public static List<TimetablePeriod> GetTimetablePeriods(int academicYearID)
        {
            return GetModelsFromDatabaseByAcademicYear<TimetablePeriod>("TimetablePeriods", academicYearID);
        }

        public static bool TimetablePeriodIsInDatabase(TimetablePeriod timetablePeriod, out int id)
        {
            string query = "SELECT * FROM TimetablePeriods WHERE academic_year_ID=@AcademicYearID AND week=@Week AND day=@Day AND period=@Period;";
            var parameters = new Dictionary<string, object> { {"@AcademicYearID", timetablePeriod.AcademicYearID }, { "@Week", timetablePeriod.Week }, { "@Day", timetablePeriod.Day}, { "@Period", timetablePeriod.Period} };
            var result = GetModelsFromDatabase<TimetablePeriod>(query, parameters);
            if (result.Any())
            {
                id = result.First().ID;
                return true;
            }
            else
            {
                id = -1;
                return false;
            }
            
        }

        public static bool TryAddTimetablePeriod(TimetablePeriod timetablePeriod, out int id)
        {
            var query = "INSERT INTO TimetablePeriods (academic_year_id, week, day, period, classcode, room) VALUES (@AcademicYearID, @Week, @Day, @Period, @ClassCode, @RoomCode);";
            var result = InsertModelsIntoDatabase(query, timetablePeriod);
            id = GetLastIDFromDatabase("TimetablePeriods");
            return result;
        }

        public static bool TryUpdateTimetablePeriod(TimetablePeriod timetablePeriod)
        {
            var query = "UPDATE TimetablePeriods SET classcode=@ClassCode, room=@RoomCode WHERE id=@ID;";
            return UpdateModelsInDatabase(query, timetablePeriod);
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

        private static bool RemoveFromDatabaseByID(string tablename, int id)
        {
            if (CheckTableExists(tablename))
            {
                var query = $"DELETE FROM {tablename} WHERE id=@ID";
                var parameters = new Dictionary<string, object> { { "@ID", id } };
                return RemoveFromDatabase(query, parameters);
            }
            return false;
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

        private static List<T> GetModelsFromDatabaseByAcademicYear<T>(string tablename, int id)
        {
            if (CheckTableExists(tablename))
            {
                string query = $"SELECT * FROM {tablename} WHERE academic_year_id=@ID;";
                var parameters = new Dictionary<string, object> { { "@ID", id } };
                return GetModelsFromDatabase<T>(query, parameters);
            }
            return new List<T>();
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
