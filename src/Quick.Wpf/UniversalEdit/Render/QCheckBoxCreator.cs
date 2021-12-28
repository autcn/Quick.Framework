using System.Windows;
using System.Windows.Controls;
using System;

namespace Quick
{
    public class QCheckBoxCreator : QEditCreatorBase<QCheckBoxAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QCheckBoxAttribute> qEditContext)
        {
            QCheckBoxAttribute attr = qEditContext.Attr;
            CheckBox checkBox = new CheckBox();
            string normalText = attr.Title.StrongTextToNormal();
            if (attr.Title.IsResourceKey())
            {
                checkBox.SetResourceReference(ContentControl.ContentProperty, normalText);
            }
            else if (attr.Title != null)
            {
                checkBox.Content = normalText;
            }
            else
            {
                checkBox.Content = qEditContext.PropertyName;
            }
            checkBox.SetBinding(CheckBox.IsCheckedProperty, CreateBinding(qEditContext));
            checkBox.IsHitTestVisible = !attr.IsReadOnly;
            checkBox.HorizontalAlignment = HorizontalAlignment.Left;
            return checkBox;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QCheckBoxAttribute> qEditContext)
        {
            bool isReadOnly = qEditContext.Attr.IsReadOnly || !qEditContext.Attr.IsEnabled;
            DataGridCheckBoxColumn chkCol = new DataGridCheckBoxColumn();
            chkCol.ElementStyle = (Style)dataGrid.FindResource(StyleKeysProperties.QDataGridCellCheckBoxStyleKey);
            chkCol.EditingElementStyle = (Style)dataGrid.FindResource(StyleKeysProperties.QDataGridCellCheckBoxEditStyleKey);
            chkCol.Binding = CreateBinding(qEditContext, true);
            chkCol.IsReadOnly = isReadOnly;
            return chkCol;
        }
    }
}
