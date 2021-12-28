using Autofac;
using System;

namespace Quick
{
    public class ServiceProviderImp : IServiceProvider
    {
        private IContainer _container;
        public ServiceProviderImp(IContainer container)
        {
            _container = container;
        }
        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }
    }
}
