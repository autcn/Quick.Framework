using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quick
{
    public class QEnumRadioSelectorCreator : QEditCreatorBase<QEnumRadioSelectorAttribute>
    {
        private readonly IEnumCollectionManager _enumCollectionManager;
        public QEnumRadioSelectorCreator(IEnumCollectionManager enumCollectionManager)
        {
            _enumCollectionManager = enumCollectionManager;
        }
        public override FrameworkElement CreateElement(QEditContext<QEnumRadioSelectorAttribute> qEditContext)
        {
            if (!qEditContext.PropertyType.GetNullableUnderlyingType().IsEnum)
            {
                throw new NotSupportedException("The QEnumRadioSelectorAttribute can be only used on enum type.");
            }
            QEnumRadioSelectorAttribute attr = qEditContext.Attr;
            RadioSelector radioSelector = new RadioSelector();
            var panel = new FrameworkElementFactory(typeof(StackPanel));
            panel.SetValue(StackPanel.OrientationProperty, attr.Orientation);
            radioSelector.ItemsPanel = new ItemsPanelTemplate(panel);
            radioSelector.ContentMemberPath = nameof(EnumItemViewModel.EnumDesc);
            radioSelector.SelectedValuePath = nameof(EnumItemViewModel.EnumValue);
            var enumCollection = _enumCollectionManager.GetEnumCollection(qEditContext.PropertyType);
            //数据源绑定
            radioSelector.SetBinding(RadioSelector.SelectedValueProperty, CreateBinding(qEditContext, true));
            radioSelector.ItemsSource = enumCollection;

            radioSelector.HorizontalContentAlignment = qEditContext.Attr.Alignment;
            return radioSelector;
        }
    }
}
