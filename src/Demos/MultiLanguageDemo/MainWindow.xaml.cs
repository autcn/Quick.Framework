using Quick;
using System.ComponentModel;
using System.Windows;
using HandyControl;

namespace MultiLanguageDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly IMessageBox _messageBox;
        private readonly MainWindowViewModel _mainVM;
        public MainWindow(IMessageBox messageBox, MainWindowViewModel mainVM)
        {
            _messageBox = messageBox;
            _mainVM = mainVM;
            InitializeComponent();
            this.DataContext = _mainVM;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //注意，当以@开头时，会从多语言资源中搜索
            if (_messageBox.QuestionOKCancel("@App.ExitWarning") == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
