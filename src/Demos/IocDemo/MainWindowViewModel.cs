using IocDemo.BLL;
using Quick;
using Serilog;
using System;

namespace IocDemo
{
    public class MainWindowViewModel : IInitializable
    {
        private readonly ILogger _logger;
        private readonly TestService1 _testService1;
        public MainWindowViewModel(ILogger logger, TestService1 testService1)
        {
            _logger = logger;
            _testService1 = testService1;
        }

        //由于注册MainWindowViewModel时，声明为可初始化，因此该方法将在实例化后执行
        public void Initialize()
        {
            _logger.Debug("MainWindowViewModel初始化!");
        }

        //由于注册MainWindowViewModel时，声明了属性注入，因此可以通过属性直接获取到容器里的服务
        //注意，属性必须有公开的set方法才能成功注入，应尽量使用构造函数注入，该模式不推荐
        public IDownloadService DownloadService { get; set; }

        public TestService3 TestService3 { get; set; }

        public void Download()
        {
            string html = DownloadService.DownloadHtml("https://www.baidu.com").Result;
            _logger.Information(html);
        }

        public void CallService()
        {
            //调用通过构造函数注入的服务 
            string name = _testService1.Print();
            _logger.Information(name);

            //用全局静态方法获取服务
            TestService2 testService2 = QServiceProvider.GetService<TestService2>();
            name = testService2.Print();
            _logger.Information(name);

            //调用通过属性注入的服务
            name = TestService3.Print();
            _logger.Information(name);

            //用全局静态方法获取服务，CalcService是通过特性标记注入的
            CalcService calcService = QServiceProvider.GetService<CalcService>();
            int result = calcService.Add(23, 24);
            _logger.Information(result.ToString());
            
            ITimeService timeService = QServiceProvider.GetService<ITimeService>();
            string curTime = timeService.GetCurrentTime();
            _logger.Information(curTime);

            IScreenService screenService = QServiceProvider.GetService<IScreenService>();
            string size = screenService.GetSize();
            _logger.Information(size);
        }
    }
}
