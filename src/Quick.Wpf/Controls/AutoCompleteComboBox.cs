using Quick;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Quick
{

    [TemplatePart(Name = ElementEditableTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementToggleButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
    public class AutoCompleteComboBox : ListBox
    {
        #region Constants

        private const string ElementEditableTextBox = "PART_EditableTextBox";
        private const string ElementToggleButton = "PART_ToggleButton";
        private const string ElementPopup = "PART_Popup";

        #endregion Constants

        public static readonly DependencyProperty FilterMemberPathProperty =
        DependencyProperty.Register("FilterMemberPath", typeof(string), typeof(AutoCompleteComboBox),
            new FrameworkPropertyMetadata(null, OnFilterMemberPathChanged));

        public string FilterMemberPath
        {
            get => (string)GetValue(FilterMemberPathProperty);
            set => SetValue(FilterMemberPathProperty, value);
        }

        private static void OnFilterMemberPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as AutoCompleteComboBox).UpdateFilterMemberPath();
        }

        public static readonly DependencyProperty CanDropDownProperty =
        DependencyProperty.Register("CanDropDown", typeof(bool), typeof(AutoCompleteComboBox),
            new FrameworkPropertyMetadata(true, OnCanDropDownChanged));

        public bool CanDropDown
        {
            get => (bool)GetValue(CanDropDownProperty);
            set => SetValue(CanDropDownProperty, value);
        }

        private static void OnCanDropDownChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as AutoCompleteComboBox).UpdateCanDropDown();
        }


        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                        "Text", typeof(string), typeof(AutoCompleteComboBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TextPropertyChangedCallback)));

        public static void TextPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as AutoCompleteComboBox).UpdateText();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(AutoCompleteComboBox), new PropertyMetadata(false));

        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }


        private bool _isExternalChanged = false;
        private bool _isEnterSelChanged = false;
        private bool _isInnerTextChanged = false;
        private TextBox _inputBox;
        private ToggleButton _toggleBtn;
        private Popup _popup;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _inputBox = (TextBox)GetTemplateChild(ElementEditableTextBox);
            _toggleBtn = (ToggleButton)GetTemplateChild(ElementToggleButton);
            if (_toggleBtn == null)
                return;
            _toggleBtn.ClickMode = ClickMode.Press;
            _popup = (Popup)GetTemplateChild(ElementPopup);

            _toggleBtn.Click += _toggleBtn_Click;
            _inputBox.TextChanged += _inputBox_TextChanged;
            _inputBox.PreviewKeyDown += _inputBox_PreviewKeyDown;
            _isExternalChanged = true;
            _inputBox.Text = Text;
            _isExternalChanged = false;
            UpdateCanDropDown();
        }
        private void UpdateText()
        {
            if(_inputBox == null)
            {
                return;
            }
            if (!_isInnerTextChanged)
            {
                _isExternalChanged = true;
                _inputBox.Text = Text;
                _isExternalChanged = false;
            }
        }

        private void UpdateCanDropDown()
        {
            if (_toggleBtn != null)
            {
                _toggleBtn.Visibility = CanDropDown ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void UpdateFilterMemberPath()
        {
            if (_entityType == null)
            {
                return;
            }
            if (FilterMemberPath != null)
            {
                _filterProperty = _entityType.GetProperty(FilterMemberPath);
            }
            else
            {
                _filterProperty = _entityType.GetProperty(DisplayMemberPath);
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            _inputBox.Focus();
            _inputBox.SelectAll();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            _isEnterSelChanged = true;
            if (SelectedItem != null)
            {
                Text = GetDisplayText(SelectedItem);
            }
            _isEnterSelChanged = false;
        }

        private string GetDisplayText(object selItem)
        {
            if (_displayProperty == null)
            {
                return selItem.ToString();
            }
            return _displayProperty.GetValue(selItem).ToString();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if (!_popup.IsOpen || !_inputBox.IsFocused)
                {
                    return;
                }
                if (Items.Count > 0)
                {
                    //如果只有一个而且选中了第一个，则关闭
                    if (SelectedIndex == 0 && Items.Count == 1)
                    {
                        _popup.IsOpen = false;
                        e.Handled = true;
                        return;
                    }

                    if (SelectedIndex < 0 || SelectedIndex == Items.Count - 1)
                        SelectedIndex = 0;
                    else if (SelectedIndex < Items.Count - 1)
                    {
                        SelectedIndex++;
                    }
                }
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        private void _inputBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!_popup.IsOpen)
            {
                return;
            }

            if (e.Key == Key.Down || e.Key == Key.Up)
            {
                if (Items.Count == 0)
                {
                    return;
                }
                if (SelectedIndex < 0)
                {
                    SelectedIndex = 0;
                }
                else
                {
                    if (e.Key == Key.Down && SelectedIndex < Items.Count - 1)
                    {
                        SelectedIndex++;
                    }
                    if (e.Key == Key.Up && SelectedIndex > 0)
                    {
                        SelectedIndex--;
                    }
                }
            }
            else if (e.Key == Key.Escape)
            {
                _popup.IsOpen = false;
            }
            else if (e.Key == Key.Enter)
            {
                if (Items.Count == 0)
                    return;

                if (SelectedIndex < 0)
                {
                    SelectedIndex = 0;
                }
                else if (SelectedIndex >= 0)
                {
                    _popup.IsOpen = false;
                }
            }
        }

        private void _inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //外部导致的变化，不予处理
            if (_isExternalChanged || _isEnterSelChanged)
            {
                _inputBox.Select(_inputBox.Text.Length, 0);
                return;
            }

            TextBox tbx = sender as TextBox;
            bool shouldOpen = false;
            if (!string.IsNullOrWhiteSpace(tbx.Text))
            {
                shouldOpen = true;
            }
            else
            {
                shouldOpen = false;
            }
            SelectedIndex = -1;
            _isInnerTextChanged = true;
            Text = tbx.Text;
            _isInnerTextChanged = false;
            if (_colView != null)
            {
                _colView.Refresh();
                if (Items.Count == 0)
                {
                    shouldOpen = false;
                }
            }
            _popup.IsOpen = shouldOpen;
        }

        private void _toggleBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton).IsChecked.Value)
            {
                _inputBox.Focus();
            }
        }

        private ICollectionView _colView;
        private IEnumerable _shadowSource;
        private PropertyInfo _filterProperty;
        private PropertyInfo _displayProperty;
        private Type _entityType;

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue == null)
            {
                _colView = null;
                _shadowSource = null;
                _entityType = null;
                _filterProperty = null;
                return;
            }

            if (_shadowSource == newValue)
            {
                return;
            }
            _colView = null;
            Type collectionType = newValue.GetType();
            _entityType = collectionType.GenericTypeArguments[0];
            UpdateFilterMemberPath();
            if (!string.IsNullOrEmpty(DisplayMemberPath))
            {
                _displayProperty = _entityType.GetProperty(DisplayMemberPath);
            }
            Type newType = typeof(ObservableCollection<>).MakeGenericType(_entityType);
            _shadowSource = (IEnumerable)Activator.CreateInstance(newType, newValue);
            _colView = CollectionViewSource.GetDefaultView(_shadowSource);
            _colView.Filter = (item) =>
            {
                if (string.IsNullOrWhiteSpace(Text))
                {
                    return true;
                }
                object val = null;
                if (_filterProperty != null)
                {
                    val = _filterProperty.GetValue(item);
                }
                else
                {
                    val = item.ToString();
                }

                if (val == null)
                {
                    return false;
                }
                IChineseToPinyin chineseToPinyin = QServiceProvider.GetService<IChineseToPinyin>();
                return chineseToPinyin.IsQuickSearchMatch(Text, val.ToString());
                //return val.ToString().ToLower().Contains(Text.ToLower());
            };
            ItemsSource = _shadowSource;

        }

    }
}
