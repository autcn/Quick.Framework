using System;
using System.Reflection;

namespace Quick
{
    public static class QEditObjectHelper
    {
        public static T CreateObject<T>()
        {
            return (T)CreateObject(typeof(T));
        }

        public static object CreateObject(Type type)
        {
            object newObj = null;
            try
            {
                newObj = QServiceProvider.GetService(type);
            }
            catch
            {

            }

            if (newObj == null)
            {
                newObj = Activator.CreateInstance(type);
            }
            return newObj;
        }

        public static T CloneWithProperties<T>(this object source)
        {
            Type type = source.GetType();
            PropertyInfo[] pInfoList = type.GetProperties();
            object newObj = CreateObject(type);
            foreach (PropertyInfo pInfo in pInfoList)
            {
                if (pInfo.CanWrite && !pInfo.IsDefined(typeof(NotCloneAttribute)))
                {
                    pInfo.SetValue(newObj, pInfo.GetValue(source));
                }
            }
            return (T)newObj;
        }

        public static object CloneWithProperties(this object source)
        {
            return source.CloneWithProperties<object>();
        }
    }
}
