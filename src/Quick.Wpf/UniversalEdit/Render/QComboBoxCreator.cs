using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{
    public class QComboBoxCreator : QEditCreatorBase<QComboBoxAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QComboBoxAttribute> qEditContext)
        {
            QComboBoxAttribute attr = qEditContext.Attr;
            ComboBox cbx = new ComboBox();

            //数据源绑定
            cbx.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(attr.ItemsSourcePath));
            bool isEditable = attr.BindType == ComboBoxBindType.Text && qEditContext.PropertyType == typeof(string);
            cbx.IsEditable = isEditable;

            //选择绑定
            BindingMode bindMode = qEditContext.Attr.BindingMode == BindingMode.Default ? BindingMode.TwoWay : qEditContext.Attr.BindingMode;
            DependencyProperty toBindProperty = null;
            if (qEditContext.PropertyType == typeof(string) && attr.BindType == ComboBoxBindType.Text)
            {
                toBindProperty = ComboBox.TextProperty;
            }
            else if (attr.BindType == ComboBoxBindType.Value)
            {
                toBindProperty = ComboBox.SelectedValueProperty;
            }
            else
            {
                toBindProperty = ComboBox.SelectedItemProperty;
            }
            cbx.SetBinding(toBindProperty, new Binding(qEditContext.PropertyName)
            {
                Mode = bindMode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true
            }); ;

            //路径设置
            if (!string.IsNullOrWhiteSpace(attr.SelectedValuePath))
            {
                cbx.SelectedValuePath = attr.SelectedValuePath;
            }

            if (!string.IsNullOrWhiteSpace(attr.DisplayMemberPath))
            {
                cbx.DisplayMemberPath = attr.DisplayMemberPath;
            }

            cbx.IsReadOnly = qEditContext.Attr.IsReadOnly;
            cbx.IsEditable = !qEditContext.Attr.IsReadOnly;
            cbx.HorizontalContentAlignment = qEditContext.Attr.Alignment;
            return cbx;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QComboBoxAttribute> qEditContext)
        {
            QComboBoxAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            DataGridColumn newCol = null;

            DataGridTemplateColumn column = new DataGridTemplateColumn();

            DataTemplate editTemplate = new DataTemplate();
            var cbx = new FrameworkElementFactory(typeof(ComboBox));

            //数据源绑定
            cbx.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(attr.ItemsSourcePath));
            bool isEditable = attr.BindType == ComboBoxBindType.Text && qEditContext.PropertyType == typeof(string);
            cbx.SetValue(ComboBox.IsEditableProperty, isEditable);

            //选择绑定
            BindingMode bindMode = qEditContext.Attr.BindingMode == BindingMode.Default ? BindingMode.TwoWay : qEditContext.Attr.BindingMode;
            DependencyProperty toBindProperty = null;
            if (qEditContext.PropertyType == typeof(string) && attr.BindType == ComboBoxBindType.Text)
            {
                toBindProperty = ComboBox.TextProperty;
            }
            else if (attr.BindType == ComboBoxBindType.Value)
            {
                toBindProperty = ComboBox.SelectedValueProperty;
            }
            else
            {
                toBindProperty = ComboBox.SelectedItemProperty;
            }

            cbx.SetBinding(toBindProperty, new Binding(qEditContext.PropertyName)
            {
                Mode = bindMode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            //路径设置
            if (!string.IsNullOrWhiteSpace(attr.SelectedValuePath))
            {
                cbx.SetValue(ComboBox.SelectedValuePathProperty, attr.SelectedValuePath);
            }

            if (!string.IsNullOrWhiteSpace(attr.DisplayMemberPath))
            {
                cbx.SetValue(ComboBox.DisplayMemberPathProperty, attr.DisplayMemberPath);
            }
            cbx.SetValue(ComboBox.HorizontalContentAlignmentProperty, TextAligmentToHAliment(attr.DataGridColumnAlignment));

            editTemplate.VisualTree = cbx;
            column.CellEditingTemplate = editTemplate;
            column.CellTemplate = CreateDisplayTempalte(dataGrid, qEditContext, toBindProperty);
            newCol = column;
            newCol.IsReadOnly = isReadOnly;
            return newCol;
        }

        private DataTemplate CreateDisplayTempalte(DataGrid dataGrid, QEditContext<QComboBoxAttribute> qEditContext, DependencyProperty toBindProperty)
        {
            DataTemplate textTemplate = new DataTemplate();
            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.TextAlignmentProperty, qEditContext.Attr.DataGridColumnAlignment);

            BindingBase bindBase = null;
            //如果绑的SelectedValue，则需要多绑定和转换器实现
            if (toBindProperty == ComboBox.SelectedValueProperty)
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
                if (toBindProperty == ComboBox.SelectedItemProperty && !qEditContext.Attr.DisplayMemberPath.IsNullOrEmpty())
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
