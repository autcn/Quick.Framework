using Autofac;
using Microsoft.Win32;
using Quick;
using MaterialThemeDemo2.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MaterialThemeDemo2
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
            _vm.DownloadFinished += _vm_DownloadFinished;
            this.DataContext = _vm;
        }

        //ViewModel下载完图片后，走一个界面动画，通过注册ViewModel的事件来实现解耦
        private void _vm_DownloadFinished(object sender, EventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = -500;
            animation.Duration = TimeSpan.FromSeconds(1);
            animation.To = 0;
            animation.FillBehavior = FillBehavior.HoldEnd;
            animation.EasingFunction = new CircleEase();
            imgTranslateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        //当需要移动界面焦点时，可以采用该方式，注意，MainWindow.cs中不能写业务代码
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _vm.Clear();
            tbxDownloadUrl.Focus();
        }

        //由于要弹出文件对话框，不便在ViewModel中弹出，因此在MainWindow.cs中弹出
        private void btnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.CanSaveImage())
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "*.jpg|*.jpg";
                if (saveFileDialog.ShowDialog().Value)
                {
                    _vm.SaveImageToFile(saveFileDialog.FileName);
                }
            }
        }

        //弹出窗口演示
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //使用Autofac原始方法，为了向构造函数传参
            LoginWindow loginWnd = QServiceProvider.GetService<LoginWindow>();

            //或LoginWindow loginWnd = new LoginWindow(loginWindowVM);

            loginWnd.Owner = this;
            if (loginWnd.ShowDialog().Value)
            {
                _vm.SetLoginResult(loginWnd.VM);
            }
        }
    }
}
