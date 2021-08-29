using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TeacherPlanner.Helpers
{
    static class RegexHelper
    {
        public const string ClassCodeOnlySaveDataPattern = @"/[^(\n)][A-Za-z0-9/]+/g";
        public const string EmptyPeriodsSaveDataPattern = "\\n``+";
        public const string EmptyNotesSaveDataPattern = "\\n +";
        public static bool SearchForCount(string regex, string input, int count)
        {
            Regex rx = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(input);
            return matches.Count == count;
        }

    }
}
