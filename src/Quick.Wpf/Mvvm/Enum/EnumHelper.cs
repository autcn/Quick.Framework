using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quick
{
    public static class EnumHelper
    {
        public static string GetEnumValueDesc(Type enumType, object value)
        {
            if (value == null)
            {
                return null;
            }
            MemberInfo[] infos = enumType.GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var info in infos)
            {
                object perVal = Enum.Parse(enumType, info.Name);
                if (object.Equals(value, perVal))
                {
                    var attr = info.GetCustomAttribute<DescriptionAttribute>();
                    if (attr != null)
                    {
                        if (QServiceProvider.TryGetService<ILocalization>(out var localization))
                        {
                            return localization.ConvertStrongText(attr.Description);
                        }
                        else
                        {
                            return attr.Description;
                        }
                    }
                    else
                    {
                        return info.Name;
                    }
                }
            }
            throw new QException("The value is not belong to the enum type.");
        }

        public static string GetEnumValueDesc<TEnum>(object value) where TEnum : Enum
        {
            return GetEnumValueDesc(typeof(TEnum), value);
        }
    }
}
