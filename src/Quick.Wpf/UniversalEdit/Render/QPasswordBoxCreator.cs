using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    public class QPasswordBoxCreator : QEditCreatorBase<QPasswordBoxAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QPasswordBoxAttribute> qEditContext)
        {
            QPasswordBoxAttribute attr = qEditContext.Attr;
            QPasswordBox passwordBox = new QPasswordBox();
            //passwordBox.IsSafeEnabled = false;
            //PasswordBoxHelper.SetAttach(passwordBox, true);
            //passwordBox.SetBinding(PasswordBoxHelper.PasswordProperty, CreateBinding(qEditContext));
            passwordBox.SetBinding(QPasswordBox.PasswordProperty, CreateBinding(qEditContext));
            //passwordBox.ShowEyeButton = attr.ShowEyeButton;
            passwordBox.ShowEyeButton = attr.ShowEyeButton;
            passwordBox.HorizontalContentAlignment = attr.Alignment;
            return passwordBox;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QPasswordBoxAttribute> qEditContext)
        {
            QPasswordBoxAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            //●
            DataGridTemplateColumn ipCol = new DataGridTemplateColumn();
            ipCol.CellTemplate = CreatePasswordTemplate(dataGrid, qEditContext);
            if (!isReadOnly)
            {
                ipCol.CellEditingTemplate = CreateEditTemplate(qEditContext, typeof(PasswordBox), PasswordBoxHelper.PasswordProperty);
                ipCol.CellEditingTemplate.VisualTree.SetValue(PasswordBoxHelper.AttachProperty, true);
                ipCol.CellEditingTemplate.VisualTree.SetValue(QPasswordBox.ShowEyeButtonProperty, qEditContext.Attr.ShowEyeButton);
                ipCol.CellEditingTemplate.VisualTree.SetValue(PasswordBox.HorizontalContentAlignmentProperty, qEditContext.Attr.Alignment);
            }
            return ipCol;
        }

        private DataTemplate CreatePasswordTemplate(DataGrid dataGrid, QEditContext<QPasswordBoxAttribute> qEditContext)
        {
            DataTemplate textTemplate = new DataTemplate();
            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.TextAlignmentProperty, qEditContext.Attr.DataGridColumnAlignment);
            textBlock.SetValue(TextBlock.TextProperty, "●●●●●●");
            textTemplate.VisualTree = textBlock;
            SetDataGridColumnTextBlockStyle(dataGrid, textBlock, qEditContext);
            return textTemplate;
        }
    }
}
