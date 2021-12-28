using System;
using System.Reflection;

namespace Quick
{
    public abstract class QModule : IQModule
    {
        protected internal bool SkipAutoServiceRegistration { get; protected set; }

        protected internal ServiceConfigurationContext ServiceConfigurationContext
        {
            get
            {
                if (_serviceConfigurationContext == null)
                {
                    throw new QException($"{nameof(ServiceConfigurationContext)} is only available in the {nameof(ConfigureServices)}, {nameof(PreConfigureServices)} and {nameof(PostConfigureServices)} methods.");
                }

                return _serviceConfigurationContext;
            }
            internal set => _serviceConfigurationContext = value;
        }

        private ServiceConfigurationContext _serviceConfigurationContext;

        public virtual void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        public virtual void ConfigureServices(ServiceConfigurationContext context)
        {

        }

        public virtual void PostConfigureServices(ServiceConfigurationContext context)
        {

        }

        public virtual void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {

        }

        public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }

        public virtual void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {

        }

        public virtual void OnApplicationShutdown(ApplicationShutdownContext context)
        {

        }

        public static bool IsSiSModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IQModule).GetTypeInfo().IsAssignableFrom(type);
        }

        internal static void CheckQModuleType(Type moduleType)
        {
            if (!IsSiSModule(moduleType))
            {
                throw new ArgumentException("Given type is not an quick module: " + moduleType.AssemblyQualifiedName);
            }
        }

        #region Configure
        public void Configure<T>(string sectionName) where T : class
        {
            ServiceConfigurationContext.ServiceBuilder.Configure<T>(sectionName);
        }

        public void Configure(Type type, string sectionName)
        {
            ServiceConfigurationContext.ServiceBuilder.Configure(type, sectionName);
        }

        public void Configure(Type type)
        {
            ServiceConfigurationContext.ServiceBuilder.Configure(type);
        }

        public void Configure(Type type, Action<object> action)
        {
            ServiceConfigurationContext.ServiceBuilder.Configure(type, action);
        }
        public void Configure<T>(Action<T> action) where T : class
        {
            ServiceConfigurationContext.ServiceBuilder.Configure(action);
        }

        public void Configure<T>() where T : class
        {
            ServiceConfigurationContext.ServiceBuilder.Configure<T>();
        }

        public void Configure(Type type, string sectionName, Action<object> action)
        {
            ServiceConfigurationContext.ServiceBuilder.Configure(type, sectionName, action);
        }
        public void Configure<T>(string sectionName, Action<T> action) where T : class
        {
            ServiceConfigurationContext.ServiceBuilder.Configure<T>(sectionName, action);
        }
        #endregion
    }
}