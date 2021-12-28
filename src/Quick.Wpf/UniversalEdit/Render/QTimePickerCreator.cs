
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{
    public class QTimeInputCreator : QEditCreatorBase<QTimeAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QTimeAttribute> qEditContext)
        {
            TimeInput timePicker = new TimeInput();
            Binding binding = CreateBinding(qEditContext);
            Type realType = qEditContext.PropertyType.GetNullableUnderlyingType();
            if (realType == typeof(DateTime))
            {
                timePicker.SetBinding(TimeInput.TimeProperty, binding);
            }
            else if(realType == typeof(string))
            {
                timePicker.SetBinding(TimeInput.TextProperty, binding);
            }
            //timePicker.TimeFormat = "HH:mm";
            //timePicker.ShowClearButton = true;
            return timePicker;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QTimeAttribute> qEditContext)
        {
            QTimeAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            Type realType = qEditContext.PropertyType.GetNullableUnderlyingType();
            DataGridColumn newCol = null;
            if (isReadOnly)
            {
                newCol = new DataGridTextColumn();
                newCol.IsReadOnly = true;
                SetReadOnlyDataGridColumnBinding(newCol as DataGridBoundColumn, qEditContext, typeof(DateTime), TimeToStringConverter.Default);
                SetDataGridColumnTextBlockStyle(dataGrid, (DataGridTextColumn)newCol, qEditContext);
            }
            else
            {
                DataGridTemplateColumn ipCol = new DataGridTemplateColumn();
                if (realType == typeof(DateTime))
                {
                    ipCol.CellTemplate = CreateTextBlockTemplate(dataGrid, qEditContext, typeof(DateTime), TimeToStringConverter.Default);
                    ipCol.CellEditingTemplate = CreateEditTemplate(qEditContext, typeof(TimeInput), TimeInput.TimeProperty,
                                                typeof(string), StringToIPAddressConverter.Default);
                }
                //else
                //{
                //    ipCol.CellTemplate = CreateTextBlockTemplate(qEditContext);
                //    ipCol.CellEditingTemplate = CreateEditTemplate(qEditContext, typeof(TimeInput), TimeInput.TextProperty);
                //}
                //ipCol.CellEditingTemplate.VisualTree.SetValue(TimeInput.TimeFormatProperty, "HH:mm");
                //ipCol.CellEditingTemplate.VisualTree.SetValue(TimeInput.ShowClearButtonProperty, true);
                newCol = ipCol;
            }

            return newCol;
        }
    }
}
