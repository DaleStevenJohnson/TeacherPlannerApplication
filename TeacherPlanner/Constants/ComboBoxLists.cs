using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Constants
{
    public class ComboBoxLists
    {
        public static List<string> HoursList { get; } = new List<string> { "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18" };
        public static List<string> MinuteList { get; } = new List<string> { "00", "05", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55" };
        public static List<string> ZeroToTwoList { get; } = new List<string> { "0", "1", "2"};
        public static List<string> ZeroToOneList { get; } = new List<string> { "0", "1" };

    }
}
