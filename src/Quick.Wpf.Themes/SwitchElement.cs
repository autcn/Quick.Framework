using System.Windows;
using System.Windows.Media;

namespace Quick
{
    public class SwitchElement : IconElement
    {
        public static readonly DependencyProperty CheckedBrushProperty = DependencyProperty.RegisterAttached(
            "CheckedBrush", typeof(Brush), typeof(SwitchElement), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetCheckedBrush(DependencyObject element, Brush value) => element.SetValue(CheckedBrushProperty, value);

        public static Brush GetCheckedBrush(DependencyObject element) => (Brush)element.GetValue(CheckedBrushProperty);


        public static readonly DependencyProperty GeometryCheckedProperty = DependencyProperty.RegisterAttached(
            "GeometryChecked", typeof(Geometry), typeof(SwitchElement), new PropertyMetadata(default(Geometry)));

        public static void SetGeometryChecked(DependencyObject element, Geometry value)
        {
            element.SetValue(GeometryCheckedProperty, value);
        }

        public static Geometry GetGeometryChecked(DependencyObject element)
        {
            return (Geometry)element.GetValue(GeometryCheckedProperty);
        }
    }
}
