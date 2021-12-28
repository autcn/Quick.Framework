using System;
using System.Linq;

namespace Quick
{
    public static class QRegisterServiceTypeFilter
    {
        public static bool HasBaseTypes(this Type srcType, params Type[] types)
        {
            foreach (Type type in types)
            {
                if (type.IsAssignableFrom(srcType))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool In(this Type srcType, params Type[] types)
        {
            return types.Contains(srcType);
        }
    }
}
