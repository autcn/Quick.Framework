using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{
    public class QLabelCreator : QEditCreatorBase<QLabelAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QLabelAttribute> qEditContext)
        {
            QLabelAttribute attr = qEditContext.Attr;
            UserControl userControl = new UserControl();
            userControl.SetBinding(ContentControl.ContentProperty, CreateBinding(qEditContext));
            userControl.VerticalAlignment = VerticalAlignment.Center;
            userControl.HorizontalContentAlignment = attr.Alignment;
            return userControl;
        }
        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QLabelAttribute> qEditContext)
        {
            DataGridTextColumn textCol = new DataGridTextColumn();
            textCol.IsReadOnly = true;
            Binding binding = new Binding(qEditContext.PropertyName);
            binding.Mode = BindingMode.OneWay;
            textCol.Binding = binding;
            SetDataGridColumnTextBlockStyle(dataGrid, textCol, qEditContext);
            return textCol;
        }
    }
}
