using Quick;
using System;
using System.Windows;

namespace ConfigurationDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppConfig _appConfig;
        private readonly IQConfiguration _qConfiguration;
        private readonly IMessageBox _messageBox;
        public MainWindow(AppConfig appConfig, 
                          IQConfiguration qConfiguration,
                          IMessageBox messageBox)
        {
            InitializeComponent();
            _qConfiguration = qConfiguration;
            _appConfig = appConfig;
            _messageBox = messageBox;

            tblClickTime.Text = _appConfig.LastClickTime;
            tbxName.Text = _appConfig.Name;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            /*1.配置文件路径说明:
                配置文件放置于应用程序基础路径下，默认目录为程序启动目录，默认文件名为appsettings.json，如果要修改默认配置，在App的ConfigureStartup
                函数中进行修改，使用以下代码：
                public partial class App : QWpfApplication
                {
                    protected override void ConfigureStartup(QApplicationCreationOptions options)
                    {
                        //修改下面配置，将基础目录设置到windows应用程序数据目录，如果不设置则默认为程序启动目录
                        options.Configuration.BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QuickDemo");

                        //修改配置文件名称
                        options.Configuration.FileName = "app.json";
                    }
               }

            /*2.配置类说明：见AppConfig类 */

            //3.直接从容器获取配置类，修改配置即可
            _appConfig.LastClickTime = DateTime.Now.ToString("u").TrimEnd('Z');
            _appConfig.Name = tbxName.Text;

            //4.调用配置类立即保存到磁盘文件, 如果不立即存盘，配置仅在内存中被修改, 应用程序正常退出时才会保存配置到磁盘文件, 但是当程序崩溃或异常退出时，配置将丢失。
            _qConfiguration.Save();

            _messageBox.Show("保存成功！");
        }
    }
}
