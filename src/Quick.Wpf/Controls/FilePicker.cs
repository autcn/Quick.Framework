using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Quick
{

    [TemplatePart(Name = ElemenTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementButton, Type = typeof(Button))]
    public class FilePicker : Control
    {
        #region Constants

        private const string ElemenTextBox = "PART_TextBox";
        private const string ElementButton = "PART_Button";

        #endregion Constants

        #region Private members
        private Button _btnBrowse;
        private TextBox _tbxPath;
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty InitialDirectoryProperty = DependencyProperty.Register("InitialDirectory", typeof(string),
            typeof(FilePicker), new FrameworkPropertyMetadata(null));

        public string InitialDirectory
        {
            get => (string)GetValue(InitialDirectoryProperty);
            set => SetValue(InitialDirectoryProperty, value);
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string),
            typeof(FilePicker), new FrameworkPropertyMetadata(null));

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public static readonly DependencyProperty OpenButtonTextProperty = DependencyProperty.Register("OpenButtonText", typeof(string),
            typeof(FilePicker), new FrameworkPropertyMetadata("...",null));

        public string OpenButtonText
        {
            get => (string)GetValue(OpenButtonTextProperty);
            set => SetValue(OpenButtonTextProperty, value);
        }

        public static readonly DependencyProperty ShowNameOnlyProperty = DependencyProperty.Register("ShowNameOnly", typeof(bool),
            typeof(FilePicker), new FrameworkPropertyMetadata(false));

        public bool ShowNameOnly
        {
            get => (bool)GetValue(ShowNameOnlyProperty);
            set => SetValue(ShowNameOnlyProperty, value);
        }

        public static readonly DependencyProperty SelectedPathProperty = DependencyProperty.Register("SelectedPath", typeof(string),
            typeof(FilePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string SelectedPath
        {
            get => (string)GetValue(SelectedPathProperty);
            set => SetValue(SelectedPathProperty, value);
        }

        public static readonly DependencyProperty IsReadOnlyProperty = TextBoxBase.IsReadOnlyProperty.AddOwner(typeof(FilePicker));
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly DependencyProperty IsFolderPickerProperty = DependencyProperty.Register("IsFolderPicker", typeof(bool),
            typeof(FilePicker), new FrameworkPropertyMetadata(false));

        public bool IsFolderPicker
        {
            get => (bool)GetValue(IsFolderPickerProperty);
            set => SetValue(IsFolderPickerProperty, value);
        }

        /// <summary>
        ///     是否显示清除按钮
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
            "ShowClearButton", typeof(bool), typeof(FilePicker), new PropertyMetadata(false));

        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }


        public static readonly DependencyProperty SelectionBrushProperty =
            TextBoxBase.SelectionBrushProperty.AddOwner(typeof(FilePicker));

        public Brush SelectionBrush
        {
            get => (Brush)GetValue(SelectionBrushProperty);
            set => SetValue(SelectionBrushProperty, value);
        }

        public static readonly DependencyProperty SelectionOpacityProperty =
           TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(FilePicker));

        public double SelectionOpacity
        {
            get => (double)GetValue(SelectionOpacityProperty);
            set => SetValue(SelectionOpacityProperty, value);
        }

        public static readonly DependencyProperty CaretBrushProperty =
            TextBoxBase.CaretBrushProperty.AddOwner(typeof(FilePicker));

        public Brush CaretBrush
        {
            get => (Brush)GetValue(CaretBrushProperty);
            set => SetValue(CaretBrushProperty, value);
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _btnBrowse = GetTemplateChild(ElementButton) as Button;
            _tbxPath = GetTemplateChild(ElemenTextBox) as TextBox;

            _btnBrowse.Click += _btnBrowse_Click;
            _tbxPath.SetBinding(TextBox.TextProperty, new Binding("SelectedPath")
            {
                Source = this,
                ValidatesOnDataErrors = true,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }

        private void _btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = IsFolderPicker;
            openFileDialog.InitialDirectory = InitialDirectory;
            openFileDialog.Multiselect = false;
            if (!IsFolderPicker && !Filter.IsNullOrEmpty())
            {
                string[] filterNames = Filter.Split('|');
                int groupCount = filterNames.Length;
                if(groupCount % 2 == 1)
                {
                    groupCount--;
                }
                for(int i = 0; i < groupCount; i += 2)
                {
                    var filter = new CommonFileDialogFilter(filterNames[i], filterNames[i + 1]);
                    openFileDialog.Filters.Add(filter);
                }
            }
            if(openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if(ShowNameOnly)
                {
                    SelectedPath = Path.GetFileName(openFileDialog.FileName);
                }
                else
                {
                    SelectedPath = openFileDialog.FileName;
                }
            }
        }
    }
}
