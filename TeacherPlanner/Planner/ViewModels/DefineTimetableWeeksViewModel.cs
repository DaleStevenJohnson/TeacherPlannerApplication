using System;
using System.Collections.Generic;
using System.IO;
using TeacherPlanner.Helpers;
using TeacherPlanner.Planner.Models;
using System.Linq;
using System.Windows.Input;

namespace TeacherPlanner.Planner.ViewModels
{
    public class DefineTimetableWeeksViewModel
    {
        public ICommand SaveTimeTableWeeksCommand;

        public DefineTimetableWeeksViewModel()
        {
            SaveTimeTableWeeksCommand = new SimpleCommand(_ => OnSaveTimeTableWeeks());

            if (TryGetTimeTableWeeks(out var dateRows))
                Rows = dateRows;
            else
                Rows = CreateTimeTableWeeks();
        }

        public IEnumerable<DateRowModel> Rows { get; }

        private bool TryGetTimeTableWeeks(out List<DateRowModel> dateRows)
        {
            
            dateRows = new List<DateRowModel>();
            return false;
#if !DEBUG
            if (!File.Exists(Path.Combine(FileHandlingHelper.LoggedInUserDataPath, "TimetableWeeks.txt")))
                return false;
#endif
            // TODO: Replace the new List with the loaded data
            var loadedDateRows = new List<DateRowModel>();
            dateRows.AddRange(from row in loadedDateRows
                              select row);
            return true;
        }

        private IEnumerable<DateRowModel> CreateTimeTableWeeks()
        {
            var schoolYear = GetSchoolYear();
            DateTime date = GetFirstMonday(schoolYear);

            var rows = new List<DateRowModel>();

            for (int i = 0; i < 50; i++)
            {
                rows.Add(new DateRowModel(date.AddDays(i * 7)));
            }

            return rows;
        }
  
        private DateTime GetFirstMonday(int schoolYear)
        {
            DateTime date = new DateTime(schoolYear, 9, 1);
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(1);
            }
            return date.AddDays(-7);
        }

        private int GetSchoolYear()
        {
            int month = Int32.Parse(DateTime.Today.ToString("MM"));
            int year = Int32.Parse(DateTime.Today.ToString("yyyy"));
            return month < 8 ? year - 1 : year;
        }

        private void OnSaveTimeTableWeeks()
        {
            // TODO: Write Rows property to file
        }
    }
}
