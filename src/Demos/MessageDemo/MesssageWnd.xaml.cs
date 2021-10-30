using Quick;
using System;
using System.Windows;

namespace MessageDemo
{
    /// <summary>
    /// MesssageWnd.xaml 的交互逻辑
    /// </summary>
    [TransientDependency]
    public partial class MesssageWnd : Window
    {
        private readonly IMessageBox _messageBox;
        public MesssageWnd(IMessageBox messageBox)
        {
            InitializeComponent();
            _messageBox = messageBox;

            Messenger.Default.Register<SearchFinishedMessage>(this, msg =>
            {
                _messageBox.Show(msg.Keyword);
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //虽然用的弱引用，最好还是显式注销，否则可能发生垃圾回收不及时，窗口关闭后仍然响应消息
            Messenger.Default.Unregister(this);
        }
    }
}
