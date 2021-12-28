using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Quick
{
    [TemplatePart(Name = ElementContentHost, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = ElementShowPasswordButton, Type = typeof(Button))]
    public class QPasswordBox : TextBox
    {
        #region Constants
        private const string ElementContentHost = "PART_ContentHost";
        private const string ElementShowPasswordButton = "PART_ShowPasswordButton";
        #endregion

        #region Properties

        public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(string), typeof(QPasswordBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ,OnPasswordChanged));

        public string Password
        {
            get
            {
                return (string)this.GetValue(PasswordProperty);
            }
            set
            {
                this.SetValue(PasswordProperty, value);
            }
        }

        public static readonly DependencyProperty PasswordCharProperty =
        DependencyProperty.Register("PasswordChar", typeof(char), typeof(QPasswordBox), new FrameworkPropertyMetadata('●', OnPasswordCharChanged));

        public char PasswordChar
        {
            get
            {
                return (char)this.GetValue(PasswordCharProperty);
            }
            set
            {
                this.SetValue(PasswordCharProperty, value);
            }
        }

        private static void OnPasswordCharChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as QPasswordBox).UpdatePassword();
        }


        public static readonly DependencyProperty ShowEyeButtonProperty =
        DependencyProperty.Register("ShowEyeButton", typeof(bool), typeof(QPasswordBox), new FrameworkPropertyMetadata(true, null));

        public bool ShowEyeButton
        {
            get
            {
                return (bool)this.GetValue(ShowEyeButtonProperty);
            }
            set
            {
                this.SetValue(ShowEyeButtonProperty, value);
            }
        }

        #endregion

        #region Private members

        private ToggleButton _showPassButton;
        private bool _innerChanged = false;

        #endregion

        #region Functions

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _showPassButton = (ToggleButton)GetTemplateChild(ElementShowPasswordButton);
            if (_showPassButton != null)
                _showPassButton.Click += _showPassBtn_Click;
        }

        private void _showPassBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdatePassword();
        }

        private static void OnPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as QPasswordBox).UpdatePassword();
        }


        private void UpdatePassword()
        {
            if (_showPassButton != null && _showPassButton.IsChecked.Value)
            {
                int cur = CaretIndex;
                _innerChanged = true;
                Text = Password;
                _innerChanged = false;
                CaretIndex = cur;
            }
            else
            {
                if (Password == null)
                {
                    _innerChanged = true;
                    Text = null;
                    _innerChanged = false;
                    return;
                }
                int cur = CaretIndex;
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < Password.Length; i++)
                {
                    builder.Append(PasswordChar);
                }
                _innerChanged = true;
                Text = builder.ToString();
                _innerChanged = false;
                CaretIndex = cur;
            }
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (_innerChanged)
            {
                return;
            }
            base.OnTextChanged(e);
            string curPassword = Password ?? "";
            foreach (TextChange change in e.Changes)
            {
                if (change.RemovedLength > 0)
                {
                    curPassword = curPassword.Remove(change.Offset, change.RemovedLength);
                }
                if (change.AddedLength > 0)
                {
                    curPassword = curPassword.Insert(change.Offset, Text.Substring(change.Offset, change.AddedLength));
                }
            }
            Password = curPassword;
        }

        #endregion
    }
}
