using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HrtzCraft.Converters
{
    public class GridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double) value;
            var param = (string) parameter;

            var gridLength = new GridLength(val);

            if (!(val <= 0)) return gridLength;

            // Set auto size is values is zero (aka no user size set)
            if (param.Equals("sideBar"))
                gridLength = new GridLength(1, GridUnitType.Star);

            if (param.Equals("consoleInputBar"))
                gridLength = GridLength.Auto;

            return gridLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (GridLength)value;

            return val.Value;
        }
    }
}
