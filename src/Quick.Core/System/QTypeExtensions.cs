using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class SiSTypeExtensions
    {
        public static readonly HashSet<Type> s_simpleTypes = new HashSet<Type>
                               {
                                   typeof(byte),
                                   typeof(sbyte),
                                   typeof(short),
                                   typeof(ushort),
                                   typeof(int),
                                   typeof(uint),
                                   typeof(long),
                                   typeof(ulong),
                                   typeof(float),
                                   typeof(double),
                                   typeof(decimal),
                                   typeof(bool),
                                   typeof(string),
                                   typeof(char),
                                   typeof(Guid),
                                   typeof(DateTime),
                                   typeof(DateTimeOffset),
                                   typeof(TimeSpan),
                                   typeof(byte[])
                               };

        private static readonly ConcurrentDictionary<Type, string> s_tablesDict =
            new ConcurrentDictionary<Type, string>();

        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <typeparamref name="TTarget"></typeparamref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/>.
        /// </summary>
        /// <typeparam name="TTarget">Target type</typeparam> (as reverse).
        public static bool IsAssignableTo<TTarget>(this Type type)
        {
            return type.IsAssignableTo(typeof(TTarget));
        }

        public static bool IsSimpleType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            type = underlyingType ?? type;

            return type.IsEnum || s_simpleTypes.Contains(type);
        }

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(p => p.Name == interfaceType.Name);
        }

        public static bool HasInterface<T>(this Type type)
        {
            return HasInterface(type, typeof(T));
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <paramref name="targetType"></paramref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/> (as reverse).
        /// </summary>
        /// <param name="type">this type</param>
        /// <param name="targetType">Target type</param>
        public static bool IsAssignableTo(this Type type, Type targetType)
        {
            return targetType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Gets all base classes of this type.
        /// </summary>
        /// <param name="type">The type to get its base classes.</param>
        /// <param name="includeObject">True, to include the standard <see cref="object"/> type in the returned array.</param>
        public static Type[] GetBaseClasses(this Type type, bool includeObject = true)
        {
            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
            return types.ToArray();
        }

        private static void AddTypeAndBaseTypesRecursively(
            List<Type> types,
            Type type,
            bool includeObject)
        {
            if (type == null)
            {
                return;
            }

            if (!includeObject && type == typeof(object))
            {
                return;
            }

            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
            types.Add(type);
        }

        /// <summary>
        /// Returns the first found method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="bindingFlags">The binding flags. Default is Public | Instance.</param>
        /// <param name="ignoreCase">Whether ignore case when comparing method name. Default is false.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodEx(
            this Type type,
            string methodName,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance,
            bool ignoreCase = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            MethodInfo result = null;

            var methods = type.GetMethods(bindingFlags);
            if (methods.Length > 0)
            {
                var ms = ignoreCase ?
                    methods.Where(m => m.Name.Equals(methodName)) :
                    methods.Where(m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));

                // get the declearing type matched first
                result = ms.Where(m => m.DeclaringType == type).FirstOrDefault();
                if (result == null)
                {
                    result = ms.FirstOrDefault();
                }
            }

            return result;
        }
        public static Type GetNullableUnderlyingType(this Type type, out bool isNullable)
        {
            Type underType = Nullable.GetUnderlyingType(type);
            isNullable = underType != null;
            return underType ?? type;
        }
        public static Type GetNullableUnderlyingType(this Type type)
        {
            return type.GetNullableUnderlyingType(out _);
        }

        public static bool IsNullable(this Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public static bool TryGetTableName(this Type type, out string tableName)
        {
            tableName = null;
            if (type != null)
            {
                tableName = s_tablesDict.GetOrAdd(type, t =>
                {
                    var attribute = Attribute.GetCustomAttribute(type, typeof(TableAttribute));
                    if (attribute is TableAttribute tableAttribute)
                    {
                        return string.IsNullOrWhiteSpace(tableAttribute.Schema) ?
                             tableAttribute.Name : $"{tableAttribute.Schema}.{tableAttribute.Name}";
                    }

                    return null;
                });
            }

            return !string.IsNullOrEmpty(tableName);
        }

        public static string GetTableName(this Type type)
        {
            return s_tablesDict.GetOrAdd(type, t =>
            {
                var attribute = Attribute.GetCustomAttribute(type, typeof(TableAttribute));
                if (attribute is TableAttribute tableAttribute)
                {
                    return string.IsNullOrWhiteSpace(tableAttribute.Schema) ?
                         tableAttribute.Name : $"{tableAttribute.Schema}.{tableAttribute.Name}";
                }

                return null;
            });
        }

        public static string GetTableNameOrDefault(this Type type)
        {
            return s_tablesDict.GetOrAdd(type, t =>
            {
                var attribute = Attribute.GetCustomAttribute(type, typeof(TableAttribute));
                if (attribute is TableAttribute tableAttribute)
                {
                    return string.IsNullOrWhiteSpace(tableAttribute.Schema) ?
                         tableAttribute.Name : $"{tableAttribute.Schema}.{tableAttribute.Name}";
                }
                return type.Name;
            });
        }
    }
}
