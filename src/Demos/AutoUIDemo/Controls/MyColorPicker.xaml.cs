using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoUIDemo.Controls
{
    /// <summary>
    /// MyColorPicker.xaml 的交互逻辑
    /// </summary>
    public partial class MyColorPicker : UserControl
    {
        public MyColorPicker()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
         "SelectedColor", typeof(string), typeof(MyColorPicker),
       new FrameworkPropertyMetadata("#FFFFFFFF", new PropertyChangedCallback(SelectedColorPropertyChangedCallback)));

        public static void SelectedColorPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as MyColorPicker).UpdateSelectedColor();
        }

        public string SelectedColor
        {
            get => (string)this.GetValue(SelectedColorProperty);
            set => this.SetValue(SelectedColorProperty, value);
        }

        public void UpdateSelectedColor()
        {
            if (SelectedColor == null)
            {
                rect.Fill = null;
            }
            else
            {
                SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(SelectedColor));
                rect.Fill = brush;
                hcColorPicker.SelectedBrush = brush;
            }
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = true;
        }

        private void ColorPicker_Canceled(object sender, EventArgs e)
        {
            popup.IsOpen = false;
        }

        private void ColorPicker_Confirmed(object sender, HandyControl.Data.FunctionEventArgs<Color> e)
        {
            rect.Fill = hcColorPicker.SelectedBrush;
            SelectedColor = hcColorPicker.SelectedBrush.Color.ToString();
            popup.IsOpen = false;
        }
    }
}
