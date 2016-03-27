using System;
using System.Globalization;
using System.Windows.Data;

namespace HrtzCraft.Converters
{
    public class MinLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentException(@"MinLengthConverter expects a value", nameof(value));

            if (parameter == null)
                throw new ArgumentException(@"MinLengthConverter expects a parameter value", nameof(parameter));

            var divideBy = int.Parse((string)parameter);

            return ((double)value / divideBy);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
