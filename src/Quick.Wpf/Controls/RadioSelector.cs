using Quick;
using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Quick
{
    public class RadioSelector : Selector, IMultiValueConverter, IValueConverter
    {

        public static readonly DependencyProperty ContentMemberPathProperty =
        DependencyProperty.Register("ContentMemberPath", typeof(string), typeof(RadioSelector), new FrameworkPropertyMetadata(null, ContentMemberPathPropertyChangedCallback));

        public string ContentMemberPath
        {
            get
            {
                return (string)this.GetValue(ContentMemberPathProperty);
            }
            set
            {
                this.SetValue(ContentMemberPathProperty, value);
            }
        }

        public static void ContentMemberPathPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as RadioSelector).UpdateContentMemberPath();
        }

        private void UpdateContentMemberPath()
        {
            SetItemTemplate();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (Items.Count > 0 && ItemsSource == null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    RadioButton radioButton = FromItem(Items[i]);
                    if (radioButton != null)
                    {
                        radioButton.SetBinding(RadioButton.IsCheckedProperty, new Binding("SelectedIndex")
                        {
                            Source = this,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.OneWay,
                            ConverterParameter = i,
                            Converter = this
                        });

                        radioButton.Click += RadioClickEventHandler;
                    }
                }
            }
        }

        private RadioButton FromItem(object item)
        {
            if (ItemsSource == null)
            {
                return item as RadioButton;
            }
            DependencyObject container = ItemContainerGenerator.ContainerFromItem(item);
            if (container == null)
            {
                return null;
            }
            return WpfHelper.VisualTreeSearchDown<RadioButton>(container);
        }

        private void SetItemTemplate()
        {
            if (ItemsSource == null)
            {
                return;
            }
            DataTemplate editTemplate = new DataTemplate();
            var radio = new FrameworkElementFactory(typeof(RadioButton));

            //数据源绑定
            Type type = ItemsSource.GetType();
            Type[] args = type.GetGenericArguments();
            bool isSimpleType = false;
            if (args.Length > 0)
            {
                isSimpleType = args[0].IsSimpleType();
            }
            if (!isSimpleType && !string.IsNullOrEmpty(ContentMemberPath))
            {
                radio.SetBinding(RadioButton.ContentProperty, new Binding(ContentMemberPath) { Mode = BindingMode.OneWay });
            }
            else
            {
                radio.SetValue(RadioButton.ContentProperty, new Binding("DataContext") { RelativeSource = RelativeSource.Self });
            }
            radio.SetValue(RadioButton.MarginProperty, new Thickness(0, 0, 10, 0));

            MultiBinding multiBinding = new MultiBinding();
            multiBinding.Mode = BindingMode.OneWay;
            multiBinding.Bindings.Add(new Binding() { RelativeSource = RelativeSource.Self });
            multiBinding.Bindings.Add(new Binding("SelectedIndex")
            {
                Source = this,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            multiBinding.Converter = this;

            radio.SetBinding(RadioButton.IsCheckedProperty, multiBinding);
            radio.AddHandler(RadioButton.ClickEvent, new RoutedEventHandler(RadioClickEventHandler));

            editTemplate.VisualTree = radio;
            this.ItemTemplate = editTemplate;
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            SetItemTemplate();
        }


        private void RadioClickEventHandler(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (ItemsSource != null)
            {
                SelectedIndex = this.Items.IndexOf(radioButton.DataContext);
            }
            else
            {
                SelectedIndex = this.Items.IndexOf(radioButton);
            }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
            {
                return false;
            }
            RadioButton radioButton = values[0] as RadioButton;
            int selIndex = (int)values[1];
            int index = Items.IndexOf(radioButton.DataContext);
            return selIndex == index;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            int selIndex = (int)value;
            int radioIndex = (int)parameter;
            return selIndex == radioIndex;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
