using Autofac;
using System;
using System.Reflection;

namespace Quick
{
    public static class ServiceBuilderExtensions
    {
        public static void RegisterGeneral(this ContainerBuilder serviceBuilder, Type[] transientBaseTypes, Type[] singletonTypes)
        {
            serviceBuilder.RegisterAssemblyTypes(Assembly.GetCallingAssembly())
                .Where(p => p.HasBaseTypes(transientBaseTypes) && !p.In(singletonTypes)).Initializable();

            serviceBuilder.RegisterAssemblyTypes(Assembly.GetCallingAssembly())
                .Where(p => p.In(singletonTypes)).SingleInstance().Initializable();
        }
    }
}
