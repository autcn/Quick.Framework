using Quick;
using Serilog;
using System;
using System.IO;
using System.Windows;

namespace LogDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger _logger;
        public MainWindow(ILogger logger)
        {
            _logger = logger;
            InitializeComponent();
        }

        private void btnWriteLog_Click(object sender, RoutedEventArgs e)
        {
            /*1.日志配置说明
            (1) 底层自动从配置文件中加载日志设置，并写入QLoggerConfiguration单例，用户可以手动修改配置文件进行设置，格式如下：

               {
                 "Log": {
                   "SinkTypes": [
                     "Console",
                     "File"
                   ],
                   "OutputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                   "ConsoleLevel": "Information",
                   "FileLevel": "Verbose",
                   "RollingInterval": "Day",
                   "FileName": "log-.txt",
                   "LogStorageDir": null
                 }
               }

             本框架采用Serilog，详细的配置说明见Serilog官网 https://github.com/serilog/serilog/wiki/Configuration-Basics

            (2) 如果需要代码修改日志配置，可以在AppModule的ConfigureServices函数中, 执行以下代码

                [DependsOn(typeof(QWpfModule))]
                public class AppModule : QModule
                {
                    public override void ConfigureServices(ServiceConfigurationContext context)
                    {
                       context.ServiceBuilder.Configure<QLoggerConfiguration>(config =>
                       {
                           config.FileLevel = LogEventLevel.Error; //修改日志输出级别
                       });
                    }   
                }

           2.日志输出路径说明
             当在日志配置中设置了，QLoggerConfiguration.LogStorageDir时，日志将写入该目录，如果未配置该项或配置为null，日志
             将写入到应用程序基础目录下的Logs目录，应用程序基础目录默认为程序启动目录，如需修改基础目录，在QWpfApplication的ConfigureStartup
             函数中执行以下代码:

            public partial class App : QWpfApplication
            {
               protected override void ConfigureStartup(QApplicationCreationOptions options)
               {
                   //修改下面配置，将基础目录设置到windows应用程序数据目录，如果不设置则默认为程序启动目录
                    options.Configuration.BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QuickDemo");
               }
            }

            */

            //一般日志输出，按等级
            _logger.Verbose("Verbose");
            _logger.Debug("Debug");
            _logger.Information("Information");
            _logger.Warning("Warning");
            _logger.Error("Error");
            _logger.Fatal("Fatal");
        }

        private void btnWriteExceptionLog_Click(object sender, RoutedEventArgs e)
        {
            //异常日志记录
            try
            {
                string name = null;
                Console.WriteLine(name.Length);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "计算名字长度失败！");
            }
        }

        private void btnTestCrashLog_Click(object sender, RoutedEventArgs e)
        {
            string name = null;
            Console.WriteLine(name.Length);
        }

        private void btnOpenLogDirectory_Click(object sender, RoutedEventArgs e)
        {
            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if(Directory.Exists(logDir))
            {
                WindowsApi.ShellOpenFile(logDir);
            }
        }
    }
}
