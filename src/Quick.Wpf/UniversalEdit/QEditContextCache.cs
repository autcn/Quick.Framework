using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Quick
{
    public static class QEditContextCache
    {
        private static ConcurrentDictionary<Type, Dictionary<string, QEditContext>> s_dict;
        public static Dictionary<string, QEditContext> GetTypeEditContextDict(Type type)
        {
            if (s_dict == null)
            {
                s_dict = new ConcurrentDictionary<Type, Dictionary<string, QEditContext>>();
            }
            if (!s_dict.ContainsKey(type))
            {
                Dictionary<string, QEditContext> uEditList = new Dictionary<string, QEditContext>();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    QEditAttribute attr = (QEditAttribute)pi.GetCustomAttribute(typeof(QEditAttribute));
                    if (attr != null)
                    {
                        QEditContext editItemContext = QEditContext.CreateGeneric(attr.GetType());
                        editItemContext.PropertyType = pi.PropertyType;
                        editItemContext.PropertyName = pi.Name;
                        editItemContext.ModelType = type;
                        editItemContext.SetAttr(attr);
                        uEditList.Add(pi.Name, editItemContext);
                    }
                }
                s_dict.TryAdd(type, uEditList);
                return uEditList;
            }
            else
            {
                return s_dict[type];
            }
        }

        public static QEditContext GetTypePropertyEditContext(Type type, string propertyName)
        {
            Dictionary<string, QEditContext> editItems = GetTypeEditContextDict(type);
            if (editItems.TryGetValue(propertyName, out QEditContext editItem))
            {
                return editItem;
            }
            return null;
        }
    }
}
