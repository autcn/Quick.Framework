using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Quick
{
    internal class ObjectToGeometryConverter : IValueConverter
    {
        public static ObjectToGeometryConverter Default { get; } = new ObjectToGeometryConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            string str = value.ToString();
            try
            {
                return Geometry.Parse(str);
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
