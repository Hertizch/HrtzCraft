using System;
using System.Globalization;
using System.Windows.Data;

namespace HrtzCraft.Converters
{
    public class PlayerPropertyIsOnlineToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var player = value.Equals(true) ? "Online".ToUpper() : "Offline".ToUpper();
            return player;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
