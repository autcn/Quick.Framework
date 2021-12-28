using System.Globalization;
using System.Linq;
using System.Reflection;

namespace System
{
    /// <summary>
    /// Extension methods for all objects.
    /// </summary>
    public static class SiSObjectExtensions
    {
        /// <summary>
        /// Used to simplify and beautify casting an object to a type. 
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.Type)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Returns the property value of an object with specified property name.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The property value.</returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            object result = null;

            if (obj != null && !string.IsNullOrEmpty(propertyName))
            {
                result = obj.GetType().GetProperty(propertyName)?.GetValue(obj);
            }

            return result;
        }

        public static TDestination ConvertPropertiesTo<TDestination>(this object source)
        {
            return (TDestination)ConvertPropertiesTo(source, typeof(TDestination));
        }

        public static object ConvertPropertiesTo(this object source, Type dstType)
        {
            Type srcType = source.GetType();
            PropertyInfo[] srcProperties = srcType.GetProperties();
            PropertyInfo[] dstProperties = dstType.GetProperties();
            object newObj = Activator.CreateInstance(dstType);
            foreach (PropertyInfo pDstInfo in dstProperties)
            {
                if (pDstInfo.CanWrite && !pDstInfo.IsDefined(typeof(NotCloneAttribute)))
                {
                    PropertyInfo srcInfo = srcProperties.FirstOrDefault(p => p.Name == pDstInfo.Name);
                    if (srcInfo != null)
                    {
                        pDstInfo.SetValue(newObj, srcInfo.GetValue(source));
                    }
                }
            }
            return newObj;
        }

        public static void CopyPropertiesTo(this object source, object dst)
        {
            PropertyInfo[] srcProperties = source.GetType().GetProperties();
            PropertyInfo[] dstProperties = dst.GetType().GetProperties();
            foreach (PropertyInfo pDstInfo in dstProperties)
            {
                if (pDstInfo.CanWrite && !pDstInfo.IsDefined(typeof(NotCloneAttribute)))
                {
                    PropertyInfo srcInfo = srcProperties.FirstOrDefault(p => p.Name == pDstInfo.Name);
                    if (srcInfo != null)
                    {
                        pDstInfo.SetValue(dst, srcInfo.GetValue(source));
                    }
                }
            }
        }

        public static T CloneProperties<T>(this object source)
        {
            Type type = source.GetType();
            PropertyInfo[] pInfoList = type.GetProperties();
            object newObj = Activator.CreateInstance(type);
            foreach (PropertyInfo pInfo in pInfoList)
            {
                if (pInfo.CanWrite && !pInfo.IsDefined(typeof(NotCloneAttribute)))
                {
                    pInfo.SetValue(newObj, pInfo.GetValue(source));
                }
            }
            return (T)newObj;
        }

        public static T CloneProperties<T>(this T source)
        {
            Type type = typeof(T);
            PropertyInfo[] pInfoList = type.GetProperties();
            object newObj = Activator.CreateInstance(type);
            foreach (PropertyInfo pInfo in pInfoList)
            {
                if (pInfo.CanWrite && !pInfo.IsDefined(typeof(NotCloneAttribute)))
                {
                    pInfo.SetValue(newObj, pInfo.GetValue(source));
                }
            }
            return (T)newObj;
        }

        public static object CloneProperties(this object source)
        {
            return CloneProperties<object>(source);
        }

        public static object ClonePropertiesByAttr<TAttribute>(this object source) where TAttribute : Attribute
        {
            return ClonePropertiesByAttr<object, TAttribute>(source);
        }

        public static TObj ClonePropertiesByAttr<TObj, TAttribute>(this object source) where TAttribute : Attribute
        {
            Type type = source.GetType();
            PropertyInfo[] pInfoList = type.GetProperties();
            object newObj = Activator.CreateInstance(type);
            foreach (PropertyInfo pInfo in pInfoList)
            {
                if (pInfo.CanWrite && pInfo.IsDefined(typeof(TAttribute)))
                {
                    pInfo.SetValue(newObj, pInfo.GetValue(source));
                }
            }
            return (TObj)newObj;
        }

        public static void CopyPropertiesToByAttr<TAttribute>(this object source, object dst) where TAttribute : Attribute
        {
            PropertyInfo[] srcProperties = source.GetType().GetProperties();
            PropertyInfo[] dstProperties = dst.GetType().GetProperties();
            foreach (PropertyInfo pDstInfo in dstProperties)
            {
                if (pDstInfo.CanWrite && pDstInfo.IsDefined(typeof(TAttribute)))
                {
                    PropertyInfo srcInfo = srcProperties.FirstOrDefault(p => p.Name == pDstInfo.Name);
                    if (srcInfo != null)
                    {
                        pDstInfo.SetValue(dst, srcInfo.GetValue(source));
                    }
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NotCloneAttribute : Attribute
    {

    }
}
