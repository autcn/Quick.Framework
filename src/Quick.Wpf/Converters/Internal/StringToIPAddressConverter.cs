using System;
using System.Globalization;
using System.Net;
using System.Windows.Data;

namespace Quick
{
    internal class StringToIPAddressConverter : IValueConverter
    {
        public static StringToIPAddressConverter Default { get; } = new StringToIPAddressConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return ((IPAddress)value).ToString();
        }
    }
}
