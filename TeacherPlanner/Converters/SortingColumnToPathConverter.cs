using System;
using System.Collections.Generic;

using System.Text;
using System.Windows;
using System.Windows.Data;
using TeacherPlanner.Constants;

namespace TeacherPlanner.Converters
{
    public class SortingColumnToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            var sortDirection = (SortDirection)value;

            return sortDirection switch
            {
                SortDirection.Ascending => "M 5,10 L 15,10 L 10,5 L 5,10",
                SortDirection.Descending => "M 5,5 L 10,10 L 15,5 L 5,5",
                _ => DependencyProperty.UnsetValue
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
