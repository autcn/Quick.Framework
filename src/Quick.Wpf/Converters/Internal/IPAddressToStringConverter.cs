using System;
using System.Globalization;
using System.Net;
using System.Windows.Data;

namespace Quick
{
    internal class IPAddressToStringConverter : IValueConverter
    {
        public static IPAddressToStringConverter Default { get; } = new IPAddressToStringConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            try
            {
                return ((IPAddress)value).ToString();
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            try
            {
                return IPAddress.Parse((string)value);
            }
            catch
            {
                return null;
            }
        }
    }
}
