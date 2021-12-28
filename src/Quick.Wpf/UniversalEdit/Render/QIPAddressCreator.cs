using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System;

namespace Quick
{
    public class QIPAddressCreator : QEditCreatorBase<QIPAddressAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QIPAddressAttribute> qEditContext)
        {
            IPAddressControl ipAddress = new IPAddressControl();
            Binding binding = CreateBinding(qEditContext);
            if (qEditContext.PropertyType == typeof(string))
            {
                binding.Converter = StringToIPAddressConverter.Default;
            }
            ipAddress.SetBinding(IPAddressControl.AddressProperty, binding);
            ipAddress.IsReadOnly = qEditContext.Attr.IsReadOnly;
            ipAddress.HorizontalAlignment = HorizontalAlignment.Left;
            return ipAddress;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QIPAddressAttribute> qEditContext)
        {
            QIPAddressAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            DataGridColumn newCol = null;
            if (isReadOnly)
            {
                newCol = new DataGridTextColumn();
                newCol.IsReadOnly = true;
                SetReadOnlyDataGridColumnBinding(newCol as DataGridBoundColumn, qEditContext, typeof(IPAddress), IPAddressToStringConverter.Default);
                SetDataGridColumnTextBlockStyle(dataGrid, (DataGridTextColumn)newCol, qEditContext);
            }
            else
            {
                DataGridTemplateColumn ipCol = new DataGridTemplateColumn();
                ipCol.CellTemplate = CreateTextBlockTemplate(dataGrid, qEditContext, typeof(IPAddress), IPAddressToStringConverter.Default);
                ipCol.CellEditingTemplate = CreateEditTemplate(qEditContext, typeof(IPAddressControl), IPAddressControl.AddressProperty,
                            typeof(string), StringToIPAddressConverter.Default);

                newCol = ipCol;
            }
            return newCol;
        }
    }
}
