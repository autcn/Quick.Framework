using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{
    public class QRadioSelectorCreator : QEditCreatorBase<QRadioSelectorAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QRadioSelectorAttribute> qEditContext)
        {
            QRadioSelectorAttribute attr = qEditContext.Attr;
            RadioSelector radioSelector = new RadioSelector();

            var panel = new FrameworkElementFactory(typeof(StackPanel));
            panel.SetValue(StackPanel.OrientationProperty, attr.Orientation);
            radioSelector.ItemsPanel = new ItemsPanelTemplate(panel);

            //数据源绑定

            radioSelector.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(attr.ItemsSourcePath));

            if (!attr.ContentMemberPath.IsNullOrEmpty())
            {
                radioSelector.ContentMemberPath = attr.ContentMemberPath;
            }

            //选择绑定
            BindingMode bindMode = qEditContext.Attr.BindingMode == BindingMode.Default ? BindingMode.TwoWay : qEditContext.Attr.BindingMode;
            DependencyProperty toBindProperty = null;
            if ((qEditContext.PropertyType.IsSimpleType() || qEditContext.PropertyType.IsEnum)
                && attr.BindType == RadioSelectorBindType.Value && !attr.SelectedValuePath.IsNullOrEmpty())
            {
                toBindProperty = RadioSelector.SelectedValueProperty;
            }
            else
            {
                toBindProperty = RadioSelector.SelectedItemProperty;
            }

            radioSelector.SetBinding(toBindProperty, new Binding(qEditContext.PropertyName)
            {
                Mode = bindMode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            //路径设置
            if (!string.IsNullOrWhiteSpace(attr.SelectedValuePath))
            {
                radioSelector.SelectedValuePath = attr.SelectedValuePath;
            }

            return radioSelector;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QRadioSelectorAttribute> qEditContext)
        {
            QRadioSelectorAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            DataGridColumn newCol = null;
            if (isReadOnly)
            {
                newCol = new DataGridTextColumn();
                newCol.IsReadOnly = true;
                SetReadOnlyDataGridColumnBinding(newCol as DataGridBoundColumn, qEditContext);
            }
            else
            {
                DataGridTemplateColumn column = new DataGridTemplateColumn();
                column.CellTemplate = CreateTextBlockTemplate(dataGrid, qEditContext);

                DataTemplate editTemplate = new DataTemplate();
                var cbx = new FrameworkElementFactory(typeof(RadioSelector));

                //数据源绑定
                cbx.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(attr.ItemsSourcePath));

                //选择绑定
                BindingMode bindMode = qEditContext.Attr.BindingMode == BindingMode.Default ? BindingMode.TwoWay : qEditContext.Attr.BindingMode;
                DependencyProperty toBindProperty = null;
                if (qEditContext.PropertyType == typeof(string) && attr.BindType == RadioSelectorBindType.Value)
                {
                    toBindProperty = RadioSelector.SelectedValueProperty;
                }
                else
                {
                    toBindProperty = RadioSelector.SelectedItemProperty;
                }

                cbx.SetBinding(toBindProperty, new Binding(qEditContext.PropertyName)
                {
                    Mode = bindMode,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

                //路径设置
                if (!string.IsNullOrWhiteSpace(attr.SelectedValuePath))
                {
                    cbx.SetValue(RadioSelector.SelectedValuePathProperty, attr.SelectedValuePath);
                }
                cbx.SetValue(RadioSelector.HorizontalContentAlignmentProperty, attr.DataGridColumnAlignment);

                editTemplate.VisualTree = cbx;
                column.CellEditingTemplate = editTemplate;

                newCol = column;
            }
            return newCol;
        }
    }
}
