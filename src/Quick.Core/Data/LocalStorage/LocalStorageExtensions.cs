using Autofac;

namespace Quick
{
    public static class LocalStorageExtensions
    {
        internal static ContainerBuilder AddLocalStorage(this ContainerBuilder serviceBuilder, string filePath)
        {
            serviceBuilder.Register(p => new LocalStorage(p.Resolve<IQConfiguration>(), filePath))
                          .As<ILocalStorage>()
                          .SingleInstance()
                          .Initializable();
            return serviceBuilder;
        }
    }
}
