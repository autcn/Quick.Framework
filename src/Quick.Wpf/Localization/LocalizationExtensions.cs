using Autofac;
using System;

namespace Quick
{
    public static class LocalizationExtensions
    {
        public static void AddLocalization(this ContainerBuilder serviceBuilder, Action<LocalizationOptions> options)
        {
            serviceBuilder.Configure(options);
            serviceBuilder.RegisterType<Localization>().As<ILocalization>().SingleInstance().Initializable();
        }
    }
}
