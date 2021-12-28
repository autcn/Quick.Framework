using Quick;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace AppDemo
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
            List<StudentSimpleItem> dataList = new List<StudentSimpleItem>();
            for (int i = 0; i < 10; i++)
            {
                dataList.Add(new StudentSimpleItem());
            }
            dataGrid.ItemsSource = dataList;
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
