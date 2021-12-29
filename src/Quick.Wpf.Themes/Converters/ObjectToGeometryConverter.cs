using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Quick
{
    internal class IsNullConverter : IValueConverter
    {
        public static IsNullConverter Default { get; } = new IsNullConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
