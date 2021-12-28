using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{
    public class QFilePickerCreator : QEditCreatorBase<QFilePickerAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QFilePickerAttribute> qEditContext)
        {
            QFilePickerAttribute attr = qEditContext.Attr;
            FilePicker filePicker = new FilePicker();
            filePicker.SetBinding(FilePicker.SelectedPathProperty, CreateBinding(qEditContext));
            filePicker.IsReadOnly = attr.IsReadOnly;
            filePicker.ShowNameOnly = attr.ShowNameOnly;
            filePicker.ShowClearButton = true;
            filePicker.Filter = attr.Filter;
            filePicker.IsFolderPicker = attr.IsFolderPicker;
            filePicker.InitialDirectory = attr.InitialDirectory;
            string normalText = attr.OpenButtonText.StrongTextToNormal();
            if (attr.OpenButtonText.IsResourceKey())
            {
                filePicker.SetResourceReference(FilePicker.OpacityMaskProperty, normalText);
            }
            else
            {
                filePicker.OpenButtonText = normalText;
            }
            return filePicker;
        }
        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QFilePickerAttribute> qEditContext)
        {
            QFilePickerAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            DataGridColumn newCol = null;

            DataGridTemplateColumn ipCol = new DataGridTemplateColumn();
            //ipCol.CellTemplate = CreateUserControlTemplate(qEditContext);
            DataTemplate txtTemplate = CreateTextBlockTemplate(dataGrid, qEditContext);
            Binding toolTipBinding = new Binding(qEditContext.PropertyName) { Mode = BindingMode.OneWay };
            txtTemplate.VisualTree.SetBinding(TextBlock.ToolTipProperty, toolTipBinding);
            ipCol.CellTemplate = txtTemplate;
            ipCol.CellEditingTemplate = CreateEditTemplate(qEditContext, typeof(FilePicker), FilePicker.SelectedPathProperty);
            ipCol.CellEditingTemplate.VisualTree.SetValue(FilePicker.ShowNameOnlyProperty, attr.ShowNameOnly);
            ipCol.CellEditingTemplate.VisualTree.SetValue(FilePicker.ShowClearButtonProperty, true);
            ipCol.CellEditingTemplate.VisualTree.SetValue(FilePicker.FilterProperty, attr.Filter);
            ipCol.CellEditingTemplate.VisualTree.SetValue(FilePicker.InitialDirectoryProperty, attr.InitialDirectory);
            ipCol.IsReadOnly = isReadOnly;
            newCol = ipCol;

            return newCol;
        }

        protected DataTemplate CreateUserControlTemplate(QEditContext<QFilePickerAttribute> qEditContext)
        {
            DataTemplate textTemplate = new DataTemplate();
            var userControl = new FrameworkElementFactory(typeof(UserControl));
            userControl.SetValue(UserControl.HorizontalContentAlignmentProperty, TextAligmentToHAliment(qEditContext.Attr.DataGridColumnAlignment));

            Binding textBinding = new Binding(qEditContext.PropertyName) { Mode = BindingMode.OneWay };
            userControl.SetBinding(UserControl.ContentProperty, textBinding);

            Binding toolTipBinding = new Binding(qEditContext.PropertyName) { Mode = BindingMode.OneWay };
            userControl.SetBinding(UserControl.ToolTipProperty, toolTipBinding);
            textTemplate.VisualTree = userControl;
            return textTemplate;
        }
    }
}
