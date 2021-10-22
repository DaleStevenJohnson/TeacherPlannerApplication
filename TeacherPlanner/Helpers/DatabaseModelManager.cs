using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Database;
using Database.DatabaseModels;
using TeacherPlanner.Constants;
using TeacherPlanner.Planner.Models;
using TeacherPlanner.Timetable.Models;

namespace TeacherPlanner.Helpers
{
    public static class DatabaseModelManager
    {
        public static Day GetDayDBModel(DateTime date, int academicYearID)
        {
            var dayDBModel = DatabaseManager.GetDay(academicYearID, date);
            if (dayDBModel == null)
            {
                var newDBDay = new Day()
                {
                    AcademicYearID = academicYearID,
                    Date = date,
                    Notes = null,
                };

                if (DatabaseManager.TryAddDay(newDBDay, out var id))
                {
                    dayDBModel = newDBDay;
                    dayDBModel.ID = id;
                }
                else
                {
                    // Todo - implement this better
                    MessageBox.Show("Error Loading Day");
                    return null;
                }
            }
            return dayDBModel;
        }

        public static ObservableCollection<DayModel> GetLessonSequenceDayModels(List<DateTime> dates, string classcode, int academicYearID, TimetableModel timetable, CalendarManager calendarManager)
        {

            var dayModels = new ObservableCollection<DayModel>();

            // Load Days from Database
            foreach (var date in dates)
            {
                var day = GetDayDBModel(date, academicYearID);
                var allPeriodModels = GetTodaysPeriodModels(day, timetable, calendarManager);
                var periodModels = new ObservableCollection<PeriodModel>();
                foreach (var period in allPeriodModels)
                {
                    // Without the below, things might get funky upon import of a timetable / defining timetable weeks.
                    // period.TimetableClasscode = GetTimetablePeriodID(date, periodCodeInt);

                    var timetableClasscode = period.GetTimetablePeriodClasscode();
                    if (timetableClasscode == classcode || period.UserEnteredClasscode == classcode)
                    {
                        periodModels.Add(period);
                    }
                }
                dayModels.Add(new DayModel(day, periodModels));
            }
            return dayModels;
        }


        public static ObservableCollection<PeriodModel> GetTodaysPeriodModels(Day dayDBModel, TimetableModel timetable, CalendarManager calendarManager)
        {
            var PERIODS = 9;
            var periodDBModels = DatabaseManager.GetPeriods(dayDBModel.ID);
            var periodModels = new ObservableCollection<PeriodModel>();

            for (var i = 0; i < PERIODS; i++)
            {
                Period periodDBModel;
                var periodCodeInt = (int)PeriodCodesConverter.ConvertIntToPeriodCodes(i);


                if (periodDBModels.Count != 0 && i < periodDBModels.Count && periodDBModels[i].PeriodNumber == periodCodeInt)
                {
                    periodDBModel = periodDBModels[i];
                    periodDBModel.TimetableClasscode = GetTimetablePeriodID(dayDBModel.Date, periodCodeInt, timetable, calendarManager);
                }
                else
                {
                    periodDBModel = new Period()
                    {
                        DayID = dayDBModel.ID,
                        TimetableClasscode = GetTimetablePeriodID(dayDBModel.Date, periodCodeInt, timetable, calendarManager),
                        UserEnteredClasscode = null,
                        PeriodNumber = periodCodeInt,
                        MarginText = null,
                        MainText = null,
                        SideText = null
                    };
                }

                var periodModel = new PeriodModel(periodDBModel);
                periodModels.Add(periodModel);
            }
            return periodModels;
        }

        public static int? GetTimetablePeriodID(DateTime date, int period, TimetableModel timetable, CalendarManager calendarManager)
        {
            if (timetable.Week1 == null || timetable.Week2 == null)
                return null;

            var day = (int)date.DayOfWeek;
            var week = calendarManager.GetWeek(date);

            if (week == 1 || week == 2)
            {
                var timetablePeriodModel = timetable.GetPeriod(week, day, (PeriodCodes)period);
                return timetablePeriodModel.ID;
            }

            return null;
        }

        public static ObservableCollection<PeriodModel> TryUpdatePeriods(ObservableCollection<PeriodModel> periods)
        {
            foreach (var period in periods)
            {
                var dbModel = period.GetDBModel();
                if (!DatabaseManager.TryUpdatePeriod(dbModel))
                {
                    if (DatabaseManager.TryAddPeriod(dbModel, out var id))
                    {
                        period.ID = id;
                    }
                    else
                    {
                        MessageBox.Show("Error saving Period to Database");
                    }
                }
            }
            return periods;
        }

        public static int? TryUpdateDay(DayModel dayModel)
        {
            var day = dayModel.GetDBModel();

            if (!DatabaseManager.TryUpdateDay(day))
            {
                if (DatabaseManager.TryAddDay(day, out var id))
                {
                    return dayModel.ID;
                }
                else
                {
                    // Todo make this better
                    MessageBox.Show("Failed to Save Day to Database");
                }
            }
            return null;
        }
    }
}
