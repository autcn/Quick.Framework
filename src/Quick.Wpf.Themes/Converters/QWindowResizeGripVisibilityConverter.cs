using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Quick
{
    internal class QWindowResizeGripVisibilityConverter : IMultiValueConverter
    {
        public static readonly QWindowResizeGripVisibilityConverter Default = new QWindowResizeGripVisibilityConverter();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || values[0] == null || values[1] == null)
            {
                return Visibility.Collapsed;
            }
            try
            {
                ResizeMode mode = (ResizeMode)values[0];
                WindowState state = (WindowState)values[1];
                if (mode != ResizeMode.CanResizeWithGrip)
                {
                    return Visibility.Collapsed;
                }
                return state == WindowState.Normal ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
