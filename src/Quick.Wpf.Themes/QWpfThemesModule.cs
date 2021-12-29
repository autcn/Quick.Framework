using Autofac;
using System.Windows;

namespace Quick
{
    [DependsOn(typeof(QuickCoreModule),
               typeof(QWpfModule))]
    public class QWpfThemesModule : QModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<QEditWindowOptions>(options => options.WindowType = typeof(QWindow));
            context.ServiceBuilder.RegisterType<QMessageBox>().As<IMessageBox>();
            context.ServiceBuilder.AddUniversalEditCreator(typeof(QWpfThemesModule).Assembly);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var windowResource = Application.Current.TryFindResource(typeof(QWindow));
            if (windowResource != null)
            {
                FrameworkElement.StyleProperty.OverrideMetadata(typeof(QWindow), new FrameworkPropertyMetadata(windowResource));
            }
        }
    }
}
