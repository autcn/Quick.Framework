using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dapper
{
    internal static class DapperCrudTypeExtensions
    {
        static readonly HashSet<Type> s_numberTypes = new HashSet<Type>
                               {
                                   typeof(long),
                                   typeof(int),
                                   typeof(uint),
                                   typeof(ulong),
                                   typeof(byte),
                                   typeof(sbyte),
                                   typeof(short),
                                   typeof(ushort)
                               };
        //You can't insert or update complex types. Lets filter them out.


        internal static bool IsNumberType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            type = underlyingType ?? type;
            return s_numberTypes.Contains(type);
        }

        internal static string CacheKey(this IEnumerable<PropertyInfo> props)
        {
            return string.Join(",", props.Select(p => p.DeclaringType.FullName + "." + p.Name).ToArray());
        }
    }
}
