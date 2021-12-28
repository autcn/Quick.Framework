using System.Windows;
using System.Windows.Media;

namespace Quick
{
    public class SwitchElement
    {
        public static readonly DependencyProperty CheckedBrushProperty = DependencyProperty.RegisterAttached(
            "CheckedBrush", typeof(Brush), typeof(SwitchElement), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetCheckedBrush(DependencyObject element, Brush value) => element.SetValue(CheckedBrushProperty, value);

        public static Brush GetCheckedBrush(DependencyObject element) => (Brush)element.GetValue(CheckedBrushProperty);

    }
}
