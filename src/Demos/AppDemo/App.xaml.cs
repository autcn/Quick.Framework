using Autofac;
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
 Quick.HandyControl UI基础支持，帮助地址：https://handyorg.github.io/handycontrol/quick_start/
 Newtonsoft.Json  基础json转换支持，帮助地址：https://www.newtonsoft.com/json/help/html/Introduction.htm
 Autofac  依赖注入基础支持，帮助地址：https://autofac.readthedocs.io/en/latest/getting-started/index.html
 AutoMapper 对象映射支持, 帮助地址：https://docs.automapper.org/en/stable/index.html
 Serilog.Sinks.File  文件日志支持，帮助地址：https://github.com/serilog/serilog/wiki/Getting-Started
 Dapper 数据库访问类库，帮助地址：https://github.com/DapperLib/Dapper    https://dapper-tutorial.net/
 DevExpress.Mvvm  -- MVVM绑定增强，帮助地址：https://docs.devexpress.com/WPF/115771/mvvm-framework/dxbinding/dxbinding
 PropertyChanged.Fody -- 该包简化了通知属性的编写, 帮助地址：https://github.com/Fody/PropertyChanged/wiki/Attributes
 */

/*Demo说明：
  演示应用程序的基本构成，App类的增强
 */
namespace AppDemo
{
    /// <summary>
    /// 1.将基类改成QWpfApplication，同样的在Xaml中也要修改基类
    /// 2.在Xaml中将StartupUri="MainWindow.xaml"删除
    /// </summary>
    public partial class App : QWpfApplication
    {
        /// <summary>
        /// 以下属性也可以直接在Xaml里设置，这里是为了添加注释
        /// </summary>
        public App()
        {
            //应用程序单例，当设置该项时，在启动第二个实例时，将自动激活已启动的，阻止第二个启动，需要注意的是，不同应用应该使用不同的Id
            SingleInstanceAppId = "C620B9C4-92FF-4980-B536-6C7A52135761";

            //设置该项后，在应用退出时，将采用结束进程的方式执行快速关闭，加速程序关闭的速度，默认为false
            QuickShutdown = true;

            //设置该项后，当程序出现异常时，将阻止程序崩溃，并记录异常日志，默认为true
            PreventShutdownWhenCrashed = true;
        }

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
    [DependsOn(typeof(QWpfModule))]
    public class AppModule : QModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //添加消息框服务, 不写也可以，默认使用Quick风格
            context.ServiceBuilder.AddMessageBox(MessageBoxType.Quick);

            //注入窗口类
            context.ServiceBuilder.RegisterType<MainWindow>();
        }
    }
}
