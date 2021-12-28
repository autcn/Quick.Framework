using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{
    public class QDataGridCreator : QEditCreatorBase<QDataGridAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QDataGridAttribute> qEditContext)
        {
            var editAttr = qEditContext.Attr;
            QDataGrid dg = new QDataGrid();
            Binding binding = new Binding(qEditContext.PropertyName)
            {
                Mode = BindingMode.OneWay
            };
            dg.SetBinding(QDataGrid.ItemsSourceProperty, binding);
            
            dg.DeleteWarning = editAttr.DeleteWarning;
            dg.IsReadOnly = editAttr.IsReadOnly;
            dg.DeleteKeepSelection = editAttr.DeleteKeepSelection;
            dg.Padding = new Thickness(0);

            if (!editAttr.UseEditBar)
            {
                dg.MaxHeight = editAttr.MaxHeight;
                dg.Margin = new Thickness(8, 0, 0, 0);
                return dg;
            }

            dg.Margin = new Thickness(0, 8, 0, 0);
            Grid.SetRow(dg, 1);

            //EditBar
            QEditBar editBar = new QEditBar();
            editBar.EditableTarget = dg;
            editBar.Height = 30;
            editBar.FontSize = 16;
            editBar.EditMode = editAttr.EditBarEditMode;
            string normalText = editAttr.Title.StrongTextToNormal();
            if (editAttr.Title == null)
            {
                editBar.Content = qEditContext.PropertyName;
            }
            else if (editAttr.Title.IsResourceKey())
            {
                editBar.SetResourceReference(QEditBar.ContentProperty, normalText);
            }
            else
            {
                editBar.Content = normalText;
            }

            Grid.SetRow(editBar, 0);

            Grid grid = new Grid();
            grid.Margin = new Thickness(8, 0, 0, 0);
            grid.MaxHeight = editAttr.MaxHeight;
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(editBar);
            grid.Children.Add(dg);
            return grid;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QDataGridAttribute> qEditContext)
        {
            return null;
        }
    }
}
