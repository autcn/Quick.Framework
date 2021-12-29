using Quick;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LayoutDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly IMessageBox _messageBox;
        public MainWindow(IMessageBox messageBox)
        {
            _messageBox = messageBox;
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (_messageBox.QuestionOKCancel("确定要退出吗？") == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
