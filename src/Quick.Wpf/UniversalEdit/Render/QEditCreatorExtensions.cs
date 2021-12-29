using Autofac;
using System.Linq;
using System.Reflection;

namespace Quick
{
    public static class QEditCreatorExtensions
    {
        public static void AddUniversalEditCreator(this ContainerBuilder serviceBuilder, Assembly assembly)
        {
            serviceBuilder.RegisterAssemblyTypes(assembly)
                .Where(type => !type.IsAbstract && type.GetInterfaces().Any(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IQEditControlCreator<>)))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
