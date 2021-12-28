using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Controls.Primitives;


namespace Quick
{
    public class QToggleButtonCreator : QEditCreatorBase<QToggleButtonAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QToggleButtonAttribute> qEditContext)
        {
            QToggleButtonAttribute attr = qEditContext.Attr;
            ToggleButton tgButton = new ToggleButton();
            tgButton.MinWidth = 20;
            tgButton.SetBinding(ToggleButton.IsCheckedProperty, CreateBinding(qEditContext));
            tgButton.IsHitTestVisible = !attr.IsReadOnly;
            tgButton.HorizontalAlignment = HorizontalAlignment.Left;
            return tgButton;
        }
    }
}
