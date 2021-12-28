using System.Linq;

namespace System.Reflection
{
    /// <summary>
    /// Extensions to <see cref="MemberInfo"/>.
    /// </summary>
    public static class QMemberInfoExtensions
    {
        /// <summary>
        /// Gets a single attribute for a member.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute</typeparam>
        /// <param name="memberInfo">The member that will be checked for the attribute</param>
        /// <param name="inherit">Include inherited attributes</param>
        /// <returns>Returns the attribute object if found. Returns null if not found.</returns>
        public static TAttribute GetSingleAttributeOrNull<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();
            if (attrs.Length > 0)
            {
                return (TAttribute)attrs[0];
            }

            return default;
        }


        public static TAttribute GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type type, bool inherit = true)
            where TAttribute : Attribute
        {
            var attr = type.GetTypeInfo().GetSingleAttributeOrNull<TAttribute>();
            if (attr != null)
            {
                return attr;
            }

            if (type.GetTypeInfo().BaseType == null)
            {
                return null;
            }

            return type.GetTypeInfo().BaseType.GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(inherit);
        }

        public static Attribute GetFirstAttribute(this Type type, Type attributeType, bool inherit = true)
        {
            Attribute firstType = type.GetCustomAttribute(attributeType, inherit);
            if (firstType != null)
            {
                return firstType;
            }
            Type[] interfaceTypes = type.GetInterfaces();
            foreach (Type interfaceType in interfaceTypes)
            {
                firstType = interfaceType.GetCustomAttribute(attributeType, inherit);
                if (firstType != null)
                {
                    return firstType;
                }
            }
            return null;
        }
        public static Attribute GetFirstAttribute<TAttribute>(this Type type, bool inherit = true)
        {
            return type.GetFirstAttribute(typeof(TAttribute), inherit);
        }

    }
}
