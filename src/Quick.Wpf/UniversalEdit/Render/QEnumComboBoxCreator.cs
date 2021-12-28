using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{
    public class QEnumComboBoxCreator : QEditCreatorBase<QEnumComboBoxAttribute>
    {
        private readonly IEnumCollectionManager _enumCollectionManager;
        public QEnumComboBoxCreator(IEnumCollectionManager enumCollectionManager)
        {
            _enumCollectionManager = enumCollectionManager;
        }
        public override FrameworkElement CreateElement(QEditContext<QEnumComboBoxAttribute> qEditContext)
        {
            if (!qEditContext.PropertyType.GetNullableUnderlyingType().IsEnum)
            {
                throw new NotSupportedException("The QEnumComboBoxAttribute can be only used on enum type.");
            }
            QEnumComboBoxAttribute attr = qEditContext.Attr;
            ComboBox cbx = new ComboBox();
            cbx.DisplayMemberPath = nameof(EnumItemViewModel.EnumDesc);
            cbx.SelectedValuePath = nameof(EnumItemViewModel.EnumValue);
            var enumCollection = _enumCollectionManager.GetEnumCollection(qEditContext.PropertyType);
            //数据源绑定
            cbx.SetBinding(ComboBox.SelectedValueProperty, CreateBinding(qEditContext, true));
            cbx.ItemsSource = enumCollection;

            cbx.IsReadOnly = qEditContext.Attr.IsReadOnly;
            cbx.IsEditable = false;
            cbx.HorizontalContentAlignment = qEditContext.Attr.Alignment;

            return cbx;
        }

        public override DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<QEnumComboBoxAttribute> qEditContext)
        {
            if (!qEditContext.PropertyType.GetNullableUnderlyingType().IsEnum)
            {
                throw new NotSupportedException("The QEnumComboBoxAttribute can be only used on enum type.");
            }
            QEnumComboBoxAttribute attr = qEditContext.Attr;
            bool isReadOnly = attr.IsReadOnly || !attr.IsEnabled;
            DataGridColumn newCol = null;
            DataGridTemplateColumn column = new DataGridTemplateColumn();

            DataTemplate editTemplate = new DataTemplate();
            var cbx = new FrameworkElementFactory(typeof(ComboBox));
            cbx.SetValue(ComboBox.IsEditableProperty, false);

            //数据源绑定
            var enumCollection = _enumCollectionManager.GetEnumCollection(qEditContext.PropertyType);
            cbx.SetValue(ComboBox.ItemsSourceProperty, enumCollection);
            cbx.SetValue(ComboBox.IsReadOnlyProperty, qEditContext.Attr.IsReadOnly);
            cbx.SetValue(ComboBox.DisplayMemberPathProperty, nameof(EnumItemViewModel.EnumDesc));
            cbx.SetValue(ComboBox.SelectedValuePathProperty, nameof(EnumItemViewModel.EnumValue));

            BindingMode bindMode = qEditContext.Attr.BindingMode == BindingMode.Default ? BindingMode.TwoWay : qEditContext.Attr.BindingMode;
            cbx.SetBinding(ComboBox.SelectedValueProperty, new Binding(qEditContext.PropertyName)
            {
                Mode = bindMode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            cbx.SetValue(ComboBox.HorizontalContentAlignmentProperty, TextAligmentToHAliment(attr.DataGridColumnAlignment));

            editTemplate.VisualTree = cbx;
            column.CellEditingTemplate = editTemplate;
            column.CellTemplate = CreateDisplayTempalte(dataGrid, qEditContext, enumCollection);
            newCol = column;
            newCol.IsReadOnly = isReadOnly;
            return newCol;
        }
        private DataTemplate CreateDisplayTempalte(DataGrid dataGrid, QEditContext<QEnumComboBoxAttribute> qEditContext, ObservableCollection<EnumItemViewModel> enumDataSource)
        {
            DataTemplate textTemplate = new DataTemplate();
            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.TextAlignmentProperty, qEditContext.Attr.DataGridColumnAlignment);
            textBlock.SetBinding(TextBlock.TextProperty, new Binding(qEditContext.PropertyName)
            {
                Mode = BindingMode.OneWay,
                Converter = new EnumComboboxValueBindingConverter(enumDataSource)
            });

            textTemplate.VisualTree = textBlock;
            SetDataGridColumnTextBlockStyle(dataGrid, textBlock, qEditContext);
            return textTemplate;
        }
    }
}

