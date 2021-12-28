using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{ 
    public class DgColumnIndexToBorderConverter : IMultiValueConverter
    {
        public static readonly DgColumnIndexToBorderConverter Default = new DgColumnIndexToBorderConverter();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int index = (int)values[0];
                DataGrid dg = (DataGrid)values[1];
                if (index == -1)
                {
                    return new Thickness(0);
                }
                return new Thickness(0, 0, 1, 0);
            }
            catch
            {
                return new Thickness(0, 0, 1, 0);
            }
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
