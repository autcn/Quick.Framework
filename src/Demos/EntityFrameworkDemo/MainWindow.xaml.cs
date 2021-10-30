using EntityFrameworkDemo.ViewModel;
using Quick;
using System;
using System.Windows;

namespace EntityFrameworkDemo
{
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _vm;
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            this.DataContext = _vm;
        }
    }
}
