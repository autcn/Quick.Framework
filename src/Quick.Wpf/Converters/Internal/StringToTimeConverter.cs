using System;
using System.Globalization;
using System.Windows.Data;

namespace Quick
{
    internal class StringToTimeConverter : IValueConverter
    {
        public static StringToTimeConverter Default { get; } = new StringToTimeConverter();
        public static DateTime? StringToTime(string strTime)
        {
            try
            {
                string[] fileds = strTime.Split(':');
                if (fileds.Length == 2)
                {
                    int hour = System.Convert.ToInt32(fileds[0]);
                    int min = System.Convert.ToInt32(fileds[1]);
                    DateTime dtNow = DateTime.Now;
                    DateTime dt = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, hour, min, 0);
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return StringToTime((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            DateTime dt = (DateTime)value;
            return dt.ToString("HH:mm");
        }
    }
}
