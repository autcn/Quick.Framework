using Quick;
using System;
using System.Windows;

namespace MessageDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainVM;
        private MesssageWnd _popupWnd;
        public MainWindow(MainWindowViewModel mainVM)
        {
            InitializeComponent();
            _mainVM = mainVM;
            this.DataContext = _mainVM;

            Messenger.Default.Register<SearchFinishedMessage>(this, msg =>
            {
                tbxKeyword.Focus();
                tbxKeyword.SelectAll();
            });
        }

        private void btnPopupWindow_Click(object sender, RoutedEventArgs e)
        {
            if (_popupWnd == null)
            {
                _popupWnd = QServiceProvider.GetService<MesssageWnd>();
                _popupWnd.Closed += (s, evt) => _popupWnd = null;
                _popupWnd.Show();
            }

        }
    }

}
