using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    public class QEditCreator : QEditCreatorBase<QEditAttribute>
    {
        private readonly IServiceProvider _serviceProvider;
        public QEditCreator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        private Type GetDefaultEditAttributeType(Type propertyType)
        {
            Type realType = propertyType.GetNullableUnderlyingType();
            Type editAttrType = null;
            if (realType == typeof(IPAddress))
            {
                editAttrType = typeof(QIPAddressAttribute);
            }
            else if (realType == typeof(bool))
            {
                editAttrType = typeof(QCheckBoxAttribute);
            }
            else if (realType == typeof(DateTime))
            {
                editAttrType = typeof(QDatePickerAttribute);
            }
            else if (realType.IsEnum)
            {
                editAttrType = typeof(QEnumComboBoxAttribute);
            }
            else if (realType.IsGenericType && realType.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
            {
                Type genericType = realType.GenericTypeArguments[0];
                if (genericType.IsClass && !genericType.IsSimpleType())
                {
                    editAttrType = typeof(QDataGridAttribute);
                }
            }
            else
            {
                editAttrType = typeof(QTextBoxAttribute);
            }
            return editAttrType;
        }
        public override FrameworkElement CreateElement(QEditContext<QEditAttribute> qEditContext)
        {
            QEditAttribute attr = qEditContext.Attr;

            Type realType = qEditContext.PropertyType.GetNullableUnderlyingType();
            Type editAttrType = GetDefaultEditAttributeType(realType);
            Type creatorType = typeof(IQEditControlCreator<>).MakeGenericType(editAttrType);
            object creator = _serviceProvider.GetService(creatorType);
            MethodInfo createFun = creatorType.GetMethod(nameof(IQEditControlCreator<QEditAttribute>.CreateElement));
            QEditContext newContext = QEditContext.CreateGeneric(editAttrType);
            newContext.Import(qEditContext);
            FrameworkElement inputElement = (FrameworkElement)createFun.Invoke(creator, new object[] { newContext });
            return inputElement;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QEditAttribute> qEditContext)
        {
            QEditAttribute attr = qEditContext.Attr;

            Type realType = qEditContext.PropertyType.GetNullableUnderlyingType();
            Type editAttrType = GetDefaultEditAttributeType(realType);
            Type creatorType = typeof(IQEditControlCreator<>).MakeGenericType(editAttrType);
            object creator = _serviceProvider.GetService(creatorType);
            MethodInfo createFun = creatorType.GetMethod(nameof(IQEditControlCreator<QEditAttribute>.CreateDataGridColumn));
            QEditContext newContext = QEditContext.CreateGeneric(editAttrType);
            newContext.Import(qEditContext);
            DataGridColumn newColumn = (DataGridColumn)createFun.Invoke(creator, new object[] { dataGrid, newContext });
            return newColumn;
        }
    }
}
