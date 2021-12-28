using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quick
{
    [SingletonDependency]
    public class EnumCollectionManager : IEnumCollectionManager
    {
        private readonly ILocalization _localization;
        public EnumCollectionManager(ILocalization localization)
        {
            _localization = localization;
            s_cache = new ConcurrentDictionary<string, EnumCollectionStorage>();
        }
        private static ConcurrentDictionary<string, EnumCollectionStorage> s_cache;

        public ObservableCollection<EnumItemViewModel> GetEnumCollection(Type enumType, Func<object, bool> predicate)
        {
            Type realType = enumType.GetNullableUnderlyingType(out bool isNullable);

            Array array = Enum.GetValues(realType);
            string key = realType.Name;
            List<object> usedList = new List<object>();
            if (isNullable)
            {
                usedList.Add(null);
                key += ",";
            }
            foreach (object val in array)
            {
                if (predicate != null)
                {
                    if (!predicate(val))
                    {
                        continue;
                    }
                }
                key += "," + val.ToString();
                usedList.Add(val);
            }
            EnumCollectionStorage storage = null;
            if (s_cache.TryGetValue(key, out storage))
            {
                return storage.Collection;
            }
            storage = new EnumCollectionStorage(enumType);
            foreach (object val in usedList)
            {
                EnumItemViewModel enumItem = new EnumItemViewModel(val);
                storage.Collection.Add(enumItem);
            }
            s_cache.TryAdd(key, storage);
            _localization.RegisterCultureChangedHandler(this, l =>
            {
                if (s_cache.TryGetValue(key, out EnumCollectionStorage tempStorage))
                {
                    foreach (EnumItemViewModel enumItem in tempStorage.Collection)
                    {
                        if (enumItem.EnumValue != null)
                        {
                            string enumName = enumItem.EnumValue.ToString();
                            string descExpression = storage.DescriptionDict[enumName];
                            enumItem.EnumDesc = l.ConvertStrongText(descExpression);
                        }
                        else
                        {
                            string enumName = tempStorage.EnumType.Name;
                            string descExpression = storage.DescriptionDict[enumName];
                            enumItem.EnumDesc = l.ConvertStrongText(descExpression);
                        }
                    }
                }
            });
            return storage.Collection;
        }


        public ObservableCollection<EnumItemViewModel> GetEnumCollection(Type enumType)
        {
            return GetEnumCollection(enumType, null);
        }

        public ObservableCollection<EnumItemViewModel> GetEnumCollection<TEnum>(Func<TEnum, bool> predicate) where TEnum : Enum
        {
            return GetEnumCollection(typeof(TEnum), val =>
            {
                if (predicate == null)
                {
                    return true;
                }
                return predicate((TEnum)val);
            });
        }

        public ObservableCollection<EnumItemViewModel> GetEnumCollection<TEnum>() where TEnum : Enum
        {
            return GetEnumCollection<TEnum>(null);
        }

    }
}
