using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    public class PageItemsControl : ItemsControl
    {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
          "SelectedItem", typeof(object), typeof(PageItemsControl),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SelectedItemPropertyChangedCallback)));

        public static void SelectedItemPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as PageItemsControl).UpdateSelectedItem();
        }

        public object SelectedItem
        {
            get
            {
                return (object)this.GetValue(SelectedItemProperty);
            }
            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            if(newValue == null)
            {
                SelectedItem = null;
            }
            else
            {
                bool bFound = false;
                foreach(object obj in newValue)
                {
                    if(obj == SelectedItem)
                    {
                        bFound = true;
                    }
                }
                if(!bFound)
                {
                    SelectedItem = null;
                }
            }
            UpdateSelectedItem();
            base.OnItemsSourceChanged(oldValue, newValue);
        }

        private void UpdateSelectedItem()
        {
            if(ItemsSource == null)
            {
                return;
            }
            int index = 0;
            foreach (object obj in ItemsSource)
            {
                ContentPresenter contentPresenter = (ContentPresenter)this.ItemContainerGenerator.ContainerFromIndex(index);
                if (SelectedItem == obj)
                {
                    contentPresenter.Visibility = Visibility.Visible;
                }
                else
                {
                    contentPresenter.Visibility = Visibility.Collapsed;
                }
                index++;
            }
        }
    }
}
