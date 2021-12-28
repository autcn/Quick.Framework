using Quick;
using SimpleMvvmDemo.ViewModel;
using System.ComponentModel;
using System.Windows;

namespace SimpleMvvmDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow
    {
        public LoginWindowViewModel VM { private set; get; }
        private IMessageBox _msgBox;
        public LoginWindow(LoginWindowViewModel vm, IMessageBox msgBox)
        {
            _msgBox = msgBox;
            InitializeComponent();
            VM = vm;
            this.DataContext = VM;
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
                _msgBox.Show(error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
