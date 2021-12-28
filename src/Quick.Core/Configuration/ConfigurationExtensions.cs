using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Concurrent;

namespace Quick
{
    public static class ConfigurationExtensions
    {
        private static ConcurrentDictionary<Type, List<Action<object>>> s_actionDict = new ConcurrentDictionary<Type, List<Action<object>>>();
        internal static ContainerBuilder AddConfiguration(this ContainerBuilder serviceBuilder, string configFilePath)
        {
            serviceBuilder.Register(p => new QConfiguration(configFilePath)).As<IQConfiguration>().SingleInstance();
            return serviceBuilder;
        }
        public static ContainerBuilder Configure<T>(this ContainerBuilder serviceBuilder, string sectionName) where T : class
        {
            return serviceBuilder.Configure(typeof(T), sectionName);
        }

        public static ContainerBuilder Configure(this ContainerBuilder serviceBuilder, Type type, string sectionName)
        {
            return serviceBuilder.Configure(type, sectionName, null);
        }

        public static ContainerBuilder Configure(this ContainerBuilder serviceBuilder, Type type)
        {
            return serviceBuilder.Configure(type, (Action<object>)null);
        }

        public static ContainerBuilder Configure(this ContainerBuilder serviceBuilder, Type type, Action<object> action)
        {
            QConfigurationSectionAttribute sectionAttribute = type.GetCustomAttribute<QConfigurationSectionAttribute>();
            //if (sectionAttribute == null)
            //{
            //    throw new QException("Type must has QConfigureSectionAttribute");
            //}
            return serviceBuilder.Configure(type, sectionAttribute?.Name, action);
        }
        public static ContainerBuilder Configure<T>(this ContainerBuilder serviceBuilder, Action<T> action) where T : class
        {
            return serviceBuilder.Configure(typeof(T), obj => action?.Invoke((T)obj));
        }

        public static ContainerBuilder Configure<T>(this ContainerBuilder serviceBuilder) where T : class
        {
            return serviceBuilder.Configure((Action<T>)null);
        }

        public static ContainerBuilder Configure(this ContainerBuilder serviceBuilder, Type type, string sectionName, Action<object> action)
        {
            serviceBuilder.Register(p =>
           {
               object configObject = null;
               if (!sectionName.IsNullOrEmpty())
               {
                   var configuration = p.Resolve<IQConfiguration>();
                   configObject = configuration.GetOrCreateConfig(type, sectionName);
               }
               else
               {
                   configObject = Activator.CreateInstance(type);
               }
               //action?.Invoke(configObject);
               if (s_actionDict.TryGetValue(type, out List<Action<object>> actionList))
               {
                   foreach (var perAction in actionList)
                   {
                       perAction.Invoke(configObject);
                   }
                   s_actionDict.TryRemove(type, out _);
               }
               return configObject;
           }).As(type).SingleInstance();

            if (action != null)
            {
                List<Action<object>> existList = null;
                if (!s_actionDict.TryGetValue(type, out existList))
                {
                    existList = new List<Action<object>>();
                    s_actionDict.TryAdd(type, existList);
                }
                existList.Add(action);
            }

            return serviceBuilder;
        }
        public static ContainerBuilder Configure<T>(this ContainerBuilder serviceBuilder, string sectionName, Action<T> action) where T : class
        {
            return serviceBuilder.Configure(typeof(T), sectionName, obj => action?.Invoke((T)obj));
        }
    }
}
