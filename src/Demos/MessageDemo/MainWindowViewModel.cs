using Quick;
using Serilog;
using System;
using System.Collections.Generic;

namespace MessageDemo
{
    [SingletonDependency]
    public class MainWindowViewModel : QBindableBase
    {
        private readonly ILogger _logger;
        public MainWindowViewModel(ILogger logger)
        {
            _logger = logger;
        }

        public string Keyword { get; set; }

        public void DoSearch()
        {
            if (Keyword.IsNullOrEmpty())
            {
                return;
            }
            _logger.Information($"Search {Keyword} Done!");

            //发消息移动焦点(有更好的方式移动界面焦点，这里仅仅为了演示消息发送)
            //在窗口类中接收该消息
            Messenger.Default.Send(new SearchFinishedMessage(Keyword));

            //帮助详情参见：https://docs.devexpress.com/WPF/17474/mvvm-framework/messenger
        }
    }
}
