using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    public interface IQEditControlCreator<TAttribute> where TAttribute : QEditAttribute, new()
    {
        FrameworkElement CreateElement(QEditContext<TAttribute> editContext);
        DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<TAttribute> qEditContext);
    }
}
