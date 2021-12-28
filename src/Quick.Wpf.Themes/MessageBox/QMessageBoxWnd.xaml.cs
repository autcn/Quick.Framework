using System.Windows;
using System.Windows.Media;

namespace Quick
{
    /// <summary>
    /// Interaction logic for CustomMessageWnd.xaml
    /// </summary>
    public partial class QMessageBoxWnd
    {
        private ILocalization _localization;
        public QMessageBoxWnd(ILocalization localization, string strInfo, string strTitle, MessageBoxButton buttons, MessageBoxImage image)
        {
            _info = strInfo;
            _title = strTitle;
            _button = buttons;
            _image = image;
            _localization = localization;
            DialogResult = MessageBoxResult.Cancel;
            InitializeComponent();
            InitUI();
        }

        private string _title;
        private string _info;
        private MessageBoxButton _button;
        private MessageBoxImage _image;
        public new MessageBoxResult DialogResult { private set; get; }

        private void InitUI()
        {
            this.Title = _title ?? "";
            tbContent.Text = _info;
            var iconKey = string.Empty;
            var iconBrushKey = string.Empty;

            switch (_image)
            {
                case MessageBoxImage.Information:
                    iconKey = "InfoIcon";
                    iconBrushKey = "InfoBrush";
                    break;
                case MessageBoxImage.Question:
                    iconKey = "QuestionIcon";
                    iconBrushKey = "QuestionBrush";
                    break;
                case MessageBoxImage.Warning:
                    iconKey = "WarningIcon";
                    iconBrushKey = "WarningBrush";
                    break;
                case MessageBoxImage.Error:
                    iconKey = "ErrorIcon";
                    iconBrushKey = "ErrorBrush";
                    break;
            }

            if (!string.IsNullOrEmpty(iconKey))
            {
                iconPath.Data = (Geometry)this.FindResource(iconKey);
                iconPath.Fill = (Brush)this.FindResource(iconBrushKey);
            }

            string strLeftText = "";
            string strRightText = "";
            switch (_button)
            {
                case MessageBoxButton.OK:
                case MessageBoxButton.OKCancel:
                    strLeftText = _localization["Qf.CustomMsgbox.OkButtonText"];
                    strRightText = _localization["Qf.CustomMsgbox.CancelButtonText"];
                    break;
                case MessageBoxButton.YesNo:
                case MessageBoxButton.YesNoCancel:
                    strLeftText = _localization["Qf.CustomMsgbox.YesButtonText"];
                    strRightText = _localization["Qf.CustomMsgbox.NoButtonText"];
                    break;
            }
            btnOK.Content = strLeftText;
            btnCancel.Content = strRightText;
            switch (_button)
            {
                case MessageBoxButton.OK:
                    btnCancel.Visibility = Visibility.Collapsed;
                    break;
                default:
                    btnCancel.Visibility = Visibility.Visible;
                    SetDefaultButtonStyle(true);
                    break;
            }
        }

        private void SetDefaultButtonStyle(bool isOkDefault)
        {
            if (isOkDefault)
            {
                btnCancel.IsDefault = false;
                btnOK.IsDefault = true;
            }
            else
            {
                btnOK.IsDefault = false;
                btnCancel.IsDefault = true;
            }
            //string[] styleKeys = { "MessageBoxPrimaryButtonStyle", "MessageBoxButtonStyle" };
            //int index1 = isOkDefault ? 0 : 1;
            //int index2 = isOkDefault ? 1 : 0;

            //btnOK.Style = (Style)FindResource(styleKeys[index1]);
            //btnCancel.Style = (Style)FindResource(styleKeys[index2]);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = _button == MessageBoxButton.OKCancel ? MessageBoxResult.OK : MessageBoxResult.Yes;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = _button == MessageBoxButton.OKCancel ? MessageBoxResult.Cancel : MessageBoxResult.No;
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab && _button != MessageBoxButton.OK)
            {
                SetDefaultButtonStyle(!btnOK.IsDefault);
                e.Handled = true;
            }
        }
    }
}
