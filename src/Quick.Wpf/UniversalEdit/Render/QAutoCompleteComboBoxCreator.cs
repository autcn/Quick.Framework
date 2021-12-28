using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{
    public class QAutoCompleteComboBoxCreator : QEditCreatorBase<QAutoCompleteComboBoxAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QAutoCompleteComboBoxAttribute> qEditContext)
        {
            QAutoCompleteComboBoxAttribute attr = qEditContext.Attr;
            AutoCompleteComboBox cbx = new AutoCompleteComboBox();
            cbx.ShowClearButton = true;

            //数据源绑定
            cbx.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(attr.ItemsSourcePath));

            //选择绑定
            BindingMode bindMode = qEditContext.Attr.BindingMode == BindingMode.Default ? BindingMode.TwoWay : qEditContext.Attr.BindingMode;
            DependencyProperty toBindProperty = null;
            if (qEditContext.PropertyType == typeof(string) && attr.BindType == ComboBoxBindType.Text)
            {
                toBindProperty = AutoCompleteComboBox.TextProperty;
            }
            else if (attr.BindType == ComboBoxBindType.Value)
            {
                toBindProperty = AutoCompleteComboBox.SelectedValueProperty;
            }
            else
            {
                toBindProperty = AutoCompleteComboBox.SelectedItemProperty;
            }
            cbx.SetBinding(toBindProperty, new Binding(qEditContext.PropertyName)
            {
                Mode = bindMode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true
            });

            //路径设置
            if (!string.IsNullOrWhiteSpace(attr.SelectedValuePath))
            {
                cbx.SelectedValuePath = attr.SelectedValuePath;
            }

            if (!string.IsNullOrWhiteSpace(attr.DisplayMemberPath))
            {
                cbx.DisplayMemberPath = attr.DisplayMemberPath;
            }

            cbx.FilterMemberPath = attr.FilterMemberPath;
            cbx.CanDropDown = attr.CanDropDown;
            cbx.HorizontalContentAlignment = qEditContext.Attr.Alignment;
            
            return cbx;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QAutoCompleteComboBoxAttribute> qEditContext)
        {
            QAutoCompleteComboBoxAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            DataGridColumn newCol = null;

            DataGridTemplateColumn column = new DataGridTemplateColumn();

            DataTemplate editTemplate = new DataTemplate();
            var cbx = new FrameworkElementFactory(typeof(AutoCompleteComboBox));

            //数据源绑定
            cbx.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(attr.ItemsSourcePath));


            //选择绑定
            BindingMode bindMode = qEditContext.Attr.BindingMode == BindingMode.Default ? BindingMode.TwoWay : qEditContext.Attr.BindingMode;
            DependencyProperty toBindProperty = null;
            if (qEditContext.PropertyType == typeof(string) && attr.BindType == ComboBoxBindType.Text)
            {
                toBindProperty = AutoCompleteComboBox.TextProperty;
            }
            else if (attr.BindType == ComboBoxBindType.Value)
            {
                toBindProperty = AutoCompleteComboBox.SelectedValueProperty;
            }
            else
            {
                toBindProperty = AutoCompleteComboBox.SelectedItemProperty;
            }
            cbx.SetBinding(toBindProperty, new Binding(qEditContext.PropertyName)
            {
                Mode = bindMode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true
            });

            //路径设置
            if (!string.IsNullOrWhiteSpace(attr.SelectedValuePath))
            {
                cbx.SetValue(AutoCompleteComboBox.SelectedValuePathProperty, attr.SelectedValuePath);
            }

            if (!string.IsNullOrWhiteSpace(attr.DisplayMemberPath))
            {
                cbx.SetValue(AutoCompleteComboBox.DisplayMemberPathProperty, attr.DisplayMemberPath);
            }
            cbx.SetValue(AutoCompleteComboBox.HorizontalContentAlignmentProperty, TextAligmentToHAliment(attr.DataGridColumnAlignment));
            cbx.SetValue(AutoCompleteComboBox.FilterMemberPathProperty, attr.FilterMemberPath);
            cbx.SetValue(AutoCompleteComboBox.CanDropDownProperty, attr.CanDropDown);

            editTemplate.VisualTree = cbx;
            column.CellEditingTemplate = editTemplate;
            column.CellTemplate = CreateDisplayTempalte(dataGrid, qEditContext, toBindProperty);
            newCol = column;
            newCol.IsReadOnly = isReadOnly;
            return newCol;
        }

        private DataTemplate CreateDisplayTempalte(DataGrid dataGrid, QEditContext<QAutoCompleteComboBoxAttribute> qEditContext, DependencyProperty toBindProperty)
        {
            DataTemplate textTemplate = new DataTemplate();
            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.TextAlignmentProperty, qEditContext.Attr.DataGridColumnAlignment);

            BindingBase bindBase = null;
            //如果绑的SelectedValue，则需要多绑定和转换器实现
            if (toBindProperty == AutoCompleteComboBox.SelectedValueProperty)
            {
                MultiBinding multiBinding = new MultiBinding();
                multiBinding.Mode = BindingMode.OneWay;
                multiBinding.Bindings.Add(new Binding(qEditContext.PropertyName));
                multiBinding.Bindings.Add(new Binding(qEditContext.Attr.ItemsSourcePath));
                List<string> convertParams = new List<string>() { qEditContext.Attr.SelectedValuePath, qEditContext.Attr.DisplayMemberPath };
                multiBinding.ConverterParameter = convertParams;
                multiBinding.Converter = new ComboboxValueBindingConverter();
                bindBase = multiBinding;
            }
            else
            {
                Binding textBinding = new Binding(qEditContext.PropertyName);
                if (toBindProperty == AutoCompleteComboBox.SelectedItemProperty && !qEditContext.Attr.DisplayMemberPath.IsNullOrEmpty())
                {
                    textBinding = new Binding(qEditContext.PropertyName + $".{qEditContext.Attr.DisplayMemberPath}");
                }
                textBinding.Mode = BindingMode.OneWay;
                bindBase = textBinding;
            }


            textBlock.SetBinding(TextBlock.TextProperty, bindBase);
            SetDataGridColumnTextBlockStyle(dataGrid, textBlock, qEditContext);
            textTemplate.VisualTree = textBlock;
            return textTemplate;
        }
    }
}
