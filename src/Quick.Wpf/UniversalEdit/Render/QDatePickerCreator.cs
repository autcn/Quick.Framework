using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    public class QDatePickerCreator : QEditCreatorBase<QDatePickerAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QDatePickerAttribute> qEditContext)
        {
            DatePicker datePicker = new DatePicker();
            datePicker.VerticalContentAlignment = VerticalAlignment.Center;
            //datePicker.ShowClearButton = true;
            if (qEditContext.PropertyType == typeof(string))
            {
                datePicker.SetBinding(DatePicker.TextProperty, CreateBinding(qEditContext));
            }
            else
            {
                datePicker.SetBinding(DatePicker.SelectedDateProperty, CreateBinding(qEditContext));
            }
            return datePicker;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QDatePickerAttribute> qEditContext)
        {
            QDatePickerAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            DataGridColumn newCol = null;
            if (isReadOnly)
            {
                newCol = new DataGridTextColumn();
                newCol.IsReadOnly = true;
                if (qEditContext.PropertyType != typeof(string))
                {
                    SetReadOnlyDataGridColumnBinding(newCol as DataGridBoundColumn, qEditContext, null, null, "yyyy-MM-dd");
                }
                else
                {
                    SetReadOnlyDataGridColumnBinding(newCol as DataGridBoundColumn, qEditContext);
                }
                SetDataGridColumnTextBlockStyle(dataGrid, (DataGridTextColumn)newCol, qEditContext);
            }
            else
            {
                DataGridTemplateColumn ipCol = new DataGridTemplateColumn();
                if (qEditContext.PropertyType != typeof(string))
                {
                    ipCol.CellTemplate = CreateTextBlockTemplate(dataGrid, qEditContext, null, null, "yyyy-MM-dd");
                    ipCol.CellEditingTemplate = CreateEditTemplate(qEditContext, typeof(DatePicker), DatePicker.SelectedDateProperty);
                }
                else
                {
                    ipCol.CellTemplate = CreateTextBlockTemplate(dataGrid, qEditContext);
                    ipCol.CellEditingTemplate = CreateEditTemplate(qEditContext, typeof(DatePicker), DatePicker.TextProperty);
                }
                //ipCol.CellEditingTemplate.VisualTree.SetValue(DatePicker.ShowClearButtonProperty, true);

                newCol = ipCol;
            }
            return newCol;
        }
    }
}
