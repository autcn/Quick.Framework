using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Quick
{
    internal class EnumComboboxValueBindingConverter : IValueConverter
    {
        private ObservableCollection<EnumItemViewModel> _itemsSource;
        public EnumComboboxValueBindingConverter(ObservableCollection<EnumItemViewModel> itemsSource)
        {
            _itemsSource = itemsSource;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";
            if (_itemsSource == null || _itemsSource.Count == 0)
            {
                return result;
            }
            foreach (EnumItemViewModel item in _itemsSource)
            {
                if (object.Equals(value, item.EnumValue))
                {
                    result = item.EnumDesc;
                    break;
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
