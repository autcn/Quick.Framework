using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    public partial class QEditWindowContent : UserControl
    {
        #region Constructor
        public QEditWindowContent(Window hostWindow, object editVM)
        {
            _hostWindow = hostWindow;
            _vmType = editVM.GetType();
            if (!_vmType.IsClass)
            {
                throw new Exception("The editVM must be class");
            }
            _messageBox = QServiceProvider.GetService<IMessageBox>();
            _localization = QServiceProvider.GetService<ILocalization>();
            _editVM = editVM;
            _editObject = _editVM as IEditableObject;
            SetTitle();
            _validationObject = _editVM as IValidatableObject;
            _hostWindow.Loaded += QEditWindow_Loaded;
            InitializeComponent();
            _token = this.GetHashCode().ToString();
            _hostWindow.Content = this;
            _hostWindow.Closing += _hostWindow_Closing;
        }

        #endregion

        #region Private members
        private Window _hostWindow;
        private string _token;
        private IMessageBox _messageBox;
        private Type _vmType;
        private IEditableObject _editObject;
        private IValidatableObject _validationObject;
        private bool _isEditMode;
        private ILocalization _localization;
        #endregion

        #region Events
        public event Action<QEditControl> OnConfigEditControl;
        public event EventHandler<EventArgs> OkAgain;
        #endregion

        #region Properties

        private object _editVM;
        public object EditVM
        {
            get { return _editVM; }
        }

        public bool CloseWarning { get; set; } = false;

        public bool UseOKAndAgain { get; set; } = false;

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                if (_editObject != null)
                {
                    _editObject.IsEditMode = _isEditMode;

                }
            }
        }

        public QEditControl EditControl => qEditControl;

        #endregion

        #region Private functions
        private void QEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OnConfigEditControl?.Invoke(qEditControl);
            qEditControl.DataContext = _editVM;
            btnOkAndAgain.Visibility = UseOKAndAgain ? Visibility.Visible : Visibility.Collapsed;
            if (UseOKAndAgain)
            {
                btnOkAndAgain.IsDefault = true;
            }
            else
            {
                btnOk.IsDefault = true;
            }
            //qEditControl.FocusToFirstEdit();
        }

        private async void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            await OKOperation(false);
        }
        private async void ButtonOKAndAgain_Click(object sender, RoutedEventArgs e)
        {
            await OKOperation(true);
        }
        private void ShowErrorInfo(string strError)
        { 
            _messageBox.Show(_hostWindow, strError);
        }

        private async Task OKOperation(bool isAgain)
        {
            do
            {
                if (_validationObject == null)
                {
                    break;
                }
                string strError = _validationObject.Validate();
                if (strError == null)
                {
                    break;
                }

                ShowErrorInfo(strError);
                return;

            } while (false);

            //数据库行为
            if (_editObject != null)
            {
                try
                {
                    using (var loading = new Loading("", "509373C4-0FC9-46F7-A49C-58061E88D4F0"))
                    {
                        await _editObject.SubmitAsync();
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorInfo(ex.Message);
                    return;
                }
            }

            if (isAgain)
            {
                OkAgain?.Invoke(this, EventArgs.Empty);
                object newEditVM = EditVM.CloneWithProperties();
                _editVM = newEditVM;
                _editObject = _editVM as IEditableObject;
                _validationObject = _editVM as IValidatableObject;
                qEditControl.DataContext = _editVM;
                qEditControl.FocusToFirstEdit();
            }

            if (!isAgain)
                _hostWindow.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _hostWindow.DialogResult = false;
        }

        private bool IsWindowOk()
        {
            return _hostWindow != null && _hostWindow.DialogResult != null && _hostWindow.DialogResult.Value;
        }

        private void _hostWindow_Closing(object sender, CancelEventArgs e)
        {
            if (CloseWarning && !IsWindowOk() && _messageBox.QuestionOKCancel(_hostWindow, QLocalizationProperties.StQEditWndWithoutSavingWarning) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            loadingBox.Dispose();
        }

        private void SetTitle()
        {
            bool isEditMode = _editObject != null && _editObject.IsEditMode;
            string titleTemplateString = _localization.ConvertStrongText(isEditMode ? QLocalizationProperties.StQEditWndTitleTextEdit : QLocalizationProperties.StQEditWndTitleTextAdd);
            _hostWindow.Title = string.Format(titleTemplateString, GetEditObjectName(_vmType));
        }

        private string GetEditObjectName(Type type)
        {
            var attr = type.GetCustomAttribute<QEditObjectDescriptionAttribute>();
            if (attr == null)
            {
                string typeName = type.Name;
                int pos = typeName.LastIndexOf("ViewModel");
                if (pos >= 0)
                {
                    return typeName.Substring(0, pos);
                }
                pos = typeName.LastIndexOf("Item");
                if (pos >= 0)
                {
                    return typeName.Substring(0, pos);
                }
                return typeName;
            }
            string realName = _localization.ConvertStrongText(attr.Name);
            return realName;
        }
        #endregion

        #region Public functions
        public T GetInputElement<T>(string propertyName) where T : FrameworkElement
        {
            return qEditControl.GetInputElement<T>(propertyName);
        }

        public FrameworkElement GetInputElement(string propertyName)
        {
            return qEditControl.GetInputElement(propertyName);
        }
        #endregion
    }


}
