using System;
using System.Globalization;
using System.Windows.Data;

namespace Quick
{
    internal class TimeToStringConverter : IValueConverter
    {
        public static TimeToStringConverter Default { get; } = new TimeToStringConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return ((DateTime)value).ToString("HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return StringToTimeConverter.StringToTime((string)value);
        }
    }
}
