using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick
{
    public interface IEnumCollectionManager
    {
        ObservableCollection<EnumItemViewModel> GetEnumCollection(Type enumType);
        ObservableCollection<EnumItemViewModel> GetEnumCollection(Type enumType, Func<object, bool> predicate);
        ObservableCollection<EnumItemViewModel> GetEnumCollection<TEnum>(Func<TEnum, bool> predicate) where TEnum : Enum;
        ObservableCollection<EnumItemViewModel> GetEnumCollection<TEnum>() where TEnum : Enum;
    }
}
