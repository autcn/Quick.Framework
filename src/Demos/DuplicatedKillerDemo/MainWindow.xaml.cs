using Autofac;
using Microsoft.Win32;
using Quick;
using DuplicatedKillerDemo.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DuplicatedKillerDemo
{
    /// <summary>
    /// 本示例解决了Mvvm开发中的以下痛点：
    /// 1.ViewModel执行完毕后移动界面输入焦点问题
    /// 2.ViewModel执行完毕后发起界面动画问题
    /// 3.必须在ViewModel弹窗的问题
    /// 4.必须写ICommand的繁琐问题
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm;
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            this.DataContext = _vm;
        }

        private void btnDeleteLeft_Click(object sender, RoutedEventArgs e)
        {
            _vm.Delete(sender.GetDataContext<CompareItem>(), true);
        }

        private void btnDeleteRight_Click(object sender, RoutedEventArgs e)
        {
            _vm.Delete(sender.GetDataContext<CompareItem>(), false);

        }

        private void btnOpen1_Click(object sender, RoutedEventArgs e)
        {
            _vm.OpenDir(sender.GetDataContext<CompareItem>(), true);
        }

        private void btnOpen2_Click(object sender, RoutedEventArgs e)
        {
            _vm.OpenDir(sender.GetDataContext<CompareItem>(), false);
        }

        private void btnOpenFile1_Click(object sender, RoutedEventArgs e)
        {
            _vm.OpenFile(sender.GetDataContext<CompareItem>(), true);
        }

        private void btnOpenFile2_Click(object sender, RoutedEventArgs e)
        {
            _vm.OpenFile(sender.GetDataContext<CompareItem>(), false);
        }
    }
}
