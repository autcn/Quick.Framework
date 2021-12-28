using System;
using System.Globalization;
using System.Windows.Data;

namespace Quick
{
	public class NullableToValueConverter : IValueConverter
	{
		public object NullValue { get; set; }

		public object NoNullValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value == null)
			{
				return NullValue;
			}
			return NoNullValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
