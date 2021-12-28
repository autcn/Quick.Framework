using QuickThemeDemo.ViewModel;
using Quick;
using System;
using System.Windows;

/*工程创建说明：
 * (1)新建一个.net Framework工程，版本大于等于4.6.1
 * (2)Nuget搜索QuickFramework.Wpf，安装即可添加全部引用
 */

/*引用库说明及帮助文档：
 Quick.Core 框架核心，基于.net Standard 2.0
 Quick.Wpf  框架WPF核心，基于.net Framework 4.6.1
 Newtonsoft.Json  基础json转换支持，帮助地址：https://www.newtonsoft.com/json/help/html/Introduction.htm
 Autofac  依赖注入基础支持，帮助地址：https://autofac.readthedocs.io/en/latest/getting-started/index.html
 AutoMapper 对象映射支持, 帮助地址：https://docs.automapper.org/en/stable/index.html
 Serilog.Sinks.File  文件日志支持，帮助地址：https://github.com/serilog/serilog/wiki/Getting-Started
 Dapper 数据库访问类库，帮助地址：https://github.com/DapperLib/Dapper    https://dapper-tutorial.net/
 DevExpress.Mvvm  -- MVVM绑定增强，帮助地址：https://docs.devexpress.com/WPF/115771/mvvm-framework/dxbinding/dxbinding
 PropertyChanged.Fody -- 该包简化了通知属性的编写, 帮助地址：https://github.com/Fody/PropertyChanged/wiki/Attributes
 */

/*Demo说明：
  演示Quick框架自动UI机制
 */
namespace QuickThemeDemo
{
    /// <summary>
    /// 1.将基类改成QWpfApplication，同样的在Xaml中也要修改基类
    /// 2.在Xaml中将StartupUri="MainWindow.xaml"删除
    /// </summary>
    public partial class App : QWpfApplication
    {

        //由于框架采用模块化设计，必须设置启动模块
        public override Type StartupModuleType => typeof(AppModule);

        //为了从容器中获取窗口类，需要重写该方法来弹出窗口，需要在Xaml中将StartupUri="MainWindow.xaml"删除
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //从IOC容器中获取Mainwindow
            MainWindow mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }


    /// <summary>
    /// 应用程序主模块，用于注入该容器的服务
    /// </summary>
    [DependsOn(typeof(QWpfModule),
               typeof(QWpfThemesModule))]
    public class AppModule : QModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //添加多语言支持(必须显式指明要添加的语言文件)
            context.ServiceBuilder.AddLocalization(options =>
            {
                options.AddFile(this, "Lang.xaml");
            });

            //添加自定义控件Creator
            context.ServiceBuilder.AddUniversalEditCreator();

            //以下服务及其子类都被注入为短暂实例
            Type[] transientBaseTypes = { typeof(Window),
                                 typeof(QBindableBase) };

            //以下服务被注入为单例，不包括子类
            Type[] singletonTypes = { typeof(MainWindow),
                                      typeof(MainWindowViewModel) };

            context.ServiceBuilder.RegisterGeneral(transientBaseTypes, singletonTypes);
        }
    }
}
