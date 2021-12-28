using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    public class QSpinnerCreator : QEditCreatorBase<QSpinnerAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QSpinnerAttribute> qEditContext)
        {
            QSpinnerAttribute attr = qEditContext.Attr;
            SpinnerControl spinner = new SpinnerControl();
            spinner.SetBinding(SpinnerControl.NumberProperty, CreateBinding(qEditContext));
            spinner.MaxNumber = attr.MaxNumber;
            spinner.MinNumber = attr.MinNumber;
            return spinner;
        }
        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QSpinnerAttribute> qEditContext)
        {
            QSpinnerAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            DataGridColumn newCol = null;
            if (isReadOnly)
            {
                newCol = new DataGridTextColumn();
                newCol.IsReadOnly = true;
                SetReadOnlyDataGridColumnBinding(newCol as DataGridBoundColumn, qEditContext);
                SetDataGridColumnTextBlockStyle(dataGrid, (DataGridTextColumn)newCol, qEditContext);
            }
            else
            {
                DataGridTemplateColumn ipCol = new DataGridTemplateColumn();
                ipCol.CellTemplate = CreateTextBlockTemplate(dataGrid, qEditContext);
                ipCol.CellEditingTemplate = CreateEditTemplate(qEditContext, typeof(SpinnerControl), SpinnerControl.NumberProperty);
                ipCol.CellEditingTemplate.VisualTree.SetValue(SpinnerControl.MaxNumberProperty, attr.MaxNumber);
                ipCol.CellEditingTemplate.VisualTree.SetValue(SpinnerControl.MinNumberProperty, attr.MinNumber);
                newCol = ipCol;
            }
            return newCol;
        }
    }
}
