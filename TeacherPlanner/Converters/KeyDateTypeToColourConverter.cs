using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Converters
{
    public class KeyDateTypeToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == KeyDateTypes.Deadline.GetKeyDateTypeName())
                return "red";
            return "PaleGoldenrod";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
