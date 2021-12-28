using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace Quick
{
    public class EnumCollectionStorage
    {
        public EnumCollectionStorage(Type enumType)
        {
            Collection = new ObservableCollection<EnumItemViewModel>();
            EnumType = enumType;
            InitDescriptionDict();
        }
        public ObservableCollection<EnumItemViewModel> Collection { get; }
        public Type EnumType { get; }
        public Dictionary<string, string> DescriptionDict { get; private set; }

        private void InitDescriptionDict()
        {
            DescriptionDict = new Dictionary<string, string>();
            Type realType = EnumType.GetNullableUnderlyingType(out bool isNullable);
            if (isNullable)
            {
                var attr = realType.GetCustomAttribute<EnumNullDescriptionAttribute>();
                string desc = " ";
                if (attr != null)
                {
                    desc = attr.Description;
                }
                DescriptionDict.Add(EnumType.Name, desc);
            }
            MemberInfo[] infos = realType.GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var info in infos)
            {
                string desc = info.Name;
                var attr = info.GetCustomAttribute<DescriptionAttribute>();
                if (attr != null)
                {
                    desc = attr.Description;
                }
                DescriptionDict.Add(info.Name, desc);
            }
        }
    }
}
