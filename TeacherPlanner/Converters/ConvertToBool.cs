using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherPlanner.Converters
{
    public static class ConvertToBool
    {
        public static bool FromString(string str)
        {
            return str.ToLower() == "true";
        }
    }
}
