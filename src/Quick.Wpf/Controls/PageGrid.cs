using System;
using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    public class PageGrid : Grid
    {
        #region 依赖属性

        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
          "PageIndex", typeof(int), typeof(PageGrid),
          new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(PageIndexPropertyChangedCallback)));

        public static void PageIndexPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as PageGrid).UpdateValueToUI();
        }

        public int PageIndex
        {
            get
            {
                return (int)this.GetValue(PageIndexProperty);
            }
            set
            {
                this.SetValue(PageIndexProperty, value);
            }
        }

        public void UpdateValueToUI()
        {
            int realIndex = Math.Abs(PageIndex);
            for (int i = 0; i < Children.Count; i++)
            {
                if (i == realIndex)
                {
                    Children[i].Visibility = Visibility.Visible;
                }
                else
                {
                    Children[i].Visibility = Visibility.Collapsed;
                }
            }
        }
        #endregion
    }
}
