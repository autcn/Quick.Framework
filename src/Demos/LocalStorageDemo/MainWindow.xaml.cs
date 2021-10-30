using LocalStorageDemo.ViewModel;
using System.Windows;

namespace LocalStorageDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm;
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            this.DataContext = vm;
        }
    }
}
