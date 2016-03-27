using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace HrtzCraft.Converters
{
    public class MultiBindingValueToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var output = values.Aggregate<object, string>(null, (current, value) => current + value);
            return output;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
