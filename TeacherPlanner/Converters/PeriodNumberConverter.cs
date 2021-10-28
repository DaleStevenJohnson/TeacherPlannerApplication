using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Converters
{
    public class PeriodNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PeriodCodes)value)
            {
                case PeriodCodes.Registration1:
                    return "R";
                case PeriodCodes.Break1:
                    return "B";
                case PeriodCodes.Lunch1:
                    return "L";
                case PeriodCodes.Twilight:
                    return "T";
                default:
                    return (int)value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
