using System.Windows;

namespace IocDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainVM;
        public MainWindow(MainWindowViewModel mainVM)
        {
            InitializeComponent();
            _mainVM = mainVM;
            this.DataContext = _mainVM;
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            _mainVM.Download();
        }

        private void btnCallTestService_Click(object sender, RoutedEventArgs e)
        {
            _mainVM.CallService();
        }
    }
}
