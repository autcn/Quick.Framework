using Autofac;
using System;

namespace Quick
{
    [DependsOn(typeof(QuickCoreModule))]
    public class QWpfModule : QModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //添加多语言支持
            context.ServiceBuilder.AddLocalization(options =>
            {
                options.AddFile(this, "Lang.xaml");
            });

            //添加消息框
            context.ServiceBuilder.AddMessageBox();

            //添加通用UI渲染器
            context.ServiceBuilder.AddUniversalEditCreator(typeof(QWpfModule).Assembly);

            //添加消息处理器
            context.ServiceBuilder.Register(p => Messenger.Default).As<IMessenger>().SingleInstance();

            context.ServiceBuilder.Configure<QEditWindowOptions>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider.GetService<ILocalization>();
        }
    }
}
