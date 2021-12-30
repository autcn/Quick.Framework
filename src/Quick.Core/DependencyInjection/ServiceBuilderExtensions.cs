using Autofac;
using System;
using System.Reflection;

namespace Quick
{
    public static class ServiceBuilderExtensions
    {
        public static void RegisterGeneral(this ContainerBuilder serviceBuilder, Assembly assembly, Type[] transientBaseTypes, Type[] singletonTypes)
        {
            serviceBuilder.RegisterAssemblyTypes(assembly)
                .Where(p => p.HasBaseTypes(transientBaseTypes) && !p.In(singletonTypes)).Initializable();

            serviceBuilder.RegisterAssemblyTypes(assembly)
                .Where(p => p.In(singletonTypes)).SingleInstance().Initializable();
        }
    }
}
