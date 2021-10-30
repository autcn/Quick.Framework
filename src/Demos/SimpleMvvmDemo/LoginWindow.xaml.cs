using HandyControl.Controls;
using HandyControl.Data;
using SimpleMvvmDemo.ViewModel;
using System.ComponentModel;
using System.Windows;

namespace SimpleMvvmDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : System.Windows.Window
    {
        public LoginWindowViewModel VM { private set; get; }
        private string _token;
        public LoginWindow(LoginWindowViewModel vm)
        {
            InitializeComponent();
            VM = vm;
            this.DataContext = VM;
            _token = this.GetHashCode().ToString();
            Growl.Register(_token, growlPanel);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Growl.Unregister(_token);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string error = VM.Validate();
            if (error == null)
            {
                if (VM.Login())
                {
                    this.DialogResult = true;
                }
            }
            else
            {
                ShowErrorInfo(error);
            }
        }

        private void ShowErrorInfo(string strError)
        {
            Growl.Clear(_token);
            Growl.Error(new GrowlInfo()
            {
                Message = strError,
                ShowDateTime = false,
                Token = _token
            });
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
