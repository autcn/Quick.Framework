using Quick;

namespace System
{
    public static class QServiceProvider
    {
        public static object GetService(Type serviceType)
        {
            return QWpfApplication.ServiceProvider.GetService(serviceType);
        }

        public static TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService));
        }

        public static object LazyGetRequiredService(Type serviceType, ref object reference)
        {
            return QWpfApplication.ServiceProvider.LazyGetRequiredService(serviceType, ref reference);
        }

        public static TService LazyGetRequiredService<TService>(ref TService reference)
        {
            return QWpfApplication.ServiceProvider.LazyGetRequiredService<TService>(ref reference);
        }

        public static bool TryGetService(Type serviceType, out object service)
        {
            return QWpfApplication.ServiceProvider.TryGetService(serviceType, out service);
        }

        public static bool TryGetService<TService>(out TService service)
        {
            return QWpfApplication.ServiceProvider.TryGetService<TService>(out service);
        }
    }
}
