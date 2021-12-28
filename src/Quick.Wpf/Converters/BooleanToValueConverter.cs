using System;
using System.Globalization;
using System.Windows.Data;

namespace Quick
{
	/// <summary>
	/// Provides a conversion between a <see cref="T:System.Boolean" /> value and a target value.
	/// </summary>
	/// <typeparam name="T">The target type.</typeparam>
	public class BooleanToValueConverter : IValueConverter
	{
		/// <summary>
		/// The target value for true.
		/// </summary>
		public object TrueValue { get; set; }

		/// <summary>
		/// The target value for false.
		/// </summary>
		public object FalseValue { get; set; }

		/// <summary>
		/// Converts a value from the binding source.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value != null && (bool)value) ? this.TrueValue : this.FalseValue;
		}

		/// <summary>
		/// Converts a value back to the binding source.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null && value.Equals(this.TrueValue);
		}
	}
}
