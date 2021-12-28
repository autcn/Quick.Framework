using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System;

namespace Quick
{
    public class QSliderCreator : QEditCreatorBase<QSliderAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QSliderAttribute> qEditContext)
        {
            QSliderAttribute attr = qEditContext.Attr;
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            //数字
            UserControl curValueLbl = new UserControl();
            curValueLbl.Padding = new Thickness(6, 2, 6, 2);
            curValueLbl.VerticalContentAlignment = VerticalAlignment.Center;
            curValueLbl.BorderThickness = new Thickness(1);
            curValueLbl.BorderBrush = curValueLbl.Foreground;
            //curValueLbl.SetBinding(UserControl.BorderBrushProperty, new Binding(nameof(UserControl.Foreground)) { RelativeSource = RelativeSource.Self });
            if(!attr.ValueLabelStyleKey.IsNullOrEmpty())
            {
                curValueLbl.Style = (Style)Application.Current.FindResource(attr.ValueLabelStyleKey.StrongTextToNormal());
            }
            Binding binding = CreateBinding(qEditContext);
            binding.Mode = BindingMode.OneWay;
            curValueLbl.SetBinding(Label.ContentProperty, binding);

            //滑动条
            Slider slider = new Slider();
            slider.Minimum = attr.Min;
            slider.Maximum = attr.Max;
            slider.SetBinding(Slider.ValueProperty, CreateBinding(qEditContext, true));
            slider.VerticalAlignment = VerticalAlignment.Center;
            slider.IsMoveToPointEnabled = true;
            Grid.SetColumn(slider, 2);

            //开始值
            UserControl leftLbl = new UserControl();
            leftLbl.Padding = new Thickness(0);
            leftLbl.Margin = new Thickness(8, 0, 4, 0);
            leftLbl.VerticalContentAlignment = VerticalAlignment.Center;
            leftLbl.BorderThickness = new Thickness(0);
            leftLbl.SetBinding(ContentControl.ContentProperty, new Binding("Minimum")
            {
                Source = slider
            });
            Grid.SetColumn(leftLbl, 1);

            //结束值
            UserControl rightLbl = new UserControl();
            rightLbl.Padding = new Thickness(0);
            rightLbl.Margin = new Thickness(4, 0, 0, 0);
            rightLbl.VerticalContentAlignment = VerticalAlignment.Center;
            rightLbl.BorderThickness = new Thickness(0);
            rightLbl.SetBinding(ContentControl.ContentProperty, new Binding("Maximum")
            {
                Source = slider
            });
            Grid.SetColumn(rightLbl, 3);
            grid.Children.Add(curValueLbl);
            grid.Children.Add(leftLbl);
            grid.Children.Add(slider);
            grid.Children.Add(rightLbl);
            return grid;
        }
    }
}
