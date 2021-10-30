using Autofac;
using IocDemo.BLL;
using Quick;
using System;
using System.Reflection;
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
  演示以Autofac为基础的依赖注入系统的使用及Quick框架增强
 */
namespace IocDemo
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
    [DependsOn(typeof(QWpfModule))]
    public class AppModule : QModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //本框架使用Autofac做Ioc容器，详细使用文档见https://autofac.readthedocs.io/en/latest/getting-started/index.html
            //本例主要介绍基本使用方法和Quick框架的加强扩展用法

            //1. 手动注入窗口类，默认为临时对象
            context.ServiceBuilder.RegisterType<MainWindow>();

            //2. 手动注入MainWindowViewModel，并声明为单例，声明为可初始化
            //*Initializable()为Quick框架增强方法
            context.ServiceBuilder.RegisterType<MainWindowViewModel>()
                                  .SingleInstance()  //单例
                                  .PropertiesAutowired() //支持属性注入（可以避免向基类构造函数传过多的参数）
                                  .Initializable(); //当MainWindowViewModel实现了IInitializable接口时，第一次实例化时，自动调用初始化方法

            //注意：(1)不推荐在构造函数中写复杂的业务逻辑，有些情况下会造成死锁，初始化执行可通过IInitializable接口实现
            //     (2)属性注入默认为关闭，除非显示调用了PropertiesAutowired()，不推荐广泛使用属性注入，会降低可读性和可维护性

            //3. 手动注入IDownloadService以接口形式暴露，并单例
            context.ServiceBuilder.RegisterType<DownloadService>().As<IDownloadService>().SingleInstance();

            //4. 通过程序集扫描，把TestServiceBase的子类全部注入
            context.ServiceBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                                  .Where(p => p.HasBaseTypes(typeof(TestServiceBase)))
                                  .Initializable();

            //5. Quick框架增强注入：通过特性标记注入，无需在此声明，见CalcService类, TimeService类，ComputerInfoService类
        }
    }
}
