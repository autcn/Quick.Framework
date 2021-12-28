using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace Quick
{
    internal class ComboboxValueBindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";
            if (values == null || values.Length != 2 || values[1] == null)
            {
                return result;
            }
            List<string> converterParams = parameter as List<string>;
            if (converterParams == null || converterParams.Count != 2)
            {
                return result;
            }
            object selValue = values[0];
            IEnumerable itemsSource = values[1] as IEnumerable;
            string valueMemberName = converterParams[0];
            string displayMemberName = converterParams[1];
            Type itemType = null;
            PropertyInfo valPropertyInfo = null;
            PropertyInfo descPropertyInfo = null;
            foreach (object item in itemsSource)
            {
                if (itemType == null)
                {
                    itemType = item.GetType();
                    valPropertyInfo = itemType.GetProperty(valueMemberName);
                    descPropertyInfo = itemType.GetProperty(displayMemberName);
                    if (valPropertyInfo == null || descPropertyInfo == null)
                    {
                        return result;
                    }
                }
                object val = valPropertyInfo.GetValue(item);
                if (object.Equals(val, selValue))
                {
                    result = descPropertyInfo.GetValue(item)?.ToString();
                    break;
                }
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
