using System;
using System.Collections.Generic;

namespace TeacherPlanner.Helpers
{
    public class ClassCodeComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            // run the regex on both strings
            double? xRegexResult = RegexHelper.GetFirstNumberFromString(x);
            double? yRegexResult = RegexHelper.GetFirstNumberFromString(y);

            // check if they are both numbers
            if (xRegexResult != null && yRegexResult != null)
            {
                var xdouble = (double)xRegexResult;
                return xdouble.CompareTo((double)yRegexResult);
            }

            // otherwise return as string comparison
            return x.CompareTo(y);
        }
    }
}
