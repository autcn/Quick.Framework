using System;
using System.Globalization;
using System.Windows.Data;

namespace Quick
{ 
    internal class ObjectToStringConverter : IValueConverter
    {
        public static ObjectToStringConverter Default { get; } = new ObjectToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
