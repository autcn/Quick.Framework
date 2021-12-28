using Autofac;

namespace System
{
    public static class ServiceProviderExtensions
    {
        public static object LazyGetRequiredService(this IServiceProvider serviceProvider, Type serviceType, ref object reference)
        {
            if (reference == null)
            {
                reference = serviceProvider.GetService(serviceType);
            }
            return reference;
        }

        public static TService LazyGetRequiredService<TService>(this IServiceProvider serviceProvider, ref TService reference)
        {
            if (reference == null)
            {
                reference = serviceProvider.GetService<TService>();
            }
            return reference;
        }

        public static TService GetService<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider.GetService(typeof(TService));
        }

        public static bool TryGetService(this IServiceProvider serviceProvider, Type serviceType, out object service)
        {
            IContainer container = serviceProvider.GetService<IContainer>();
            return container.TryResolve(serviceType, out service);
        }

        public static bool TryGetService<TService>(this IServiceProvider serviceProvider, out TService service)
        {
            bool success = serviceProvider.TryGetService(typeof(TService), out object tempObj);
            service = (TService)tempObj;
            return success;
        }

        public static object GetNamedService(this IServiceProvider serviceProvider, Type serviceType, string serviceName)
        {
            IContainer container = serviceProvider.GetService<IContainer>();
            return container.ResolveNamed(serviceName, serviceType);
        }

        public static TService GetNamedService<TService>(this IServiceProvider serviceProvider, string serviceName)
        {
            IContainer container = serviceProvider.GetService<IContainer>();
            return container.ResolveNamed<TService>(serviceName);
        }

        public static bool TryGetNamedService(this IServiceProvider serviceProvider, Type serviceType, string serviceName, out object service)
        {
            IContainer container = serviceProvider.GetService<IContainer>();
            return container.TryResolveNamed(serviceName, serviceType, out service);
        }

        public static bool TryGetNamedService<TService>(this IServiceProvider serviceProvider, string serviceName, out TService service)
        {
            bool success = serviceProvider.TryGetNamedService(typeof(TService), serviceName, out object tempObj);
            service = (TService)tempObj;
            return success;
        }
    }
}
