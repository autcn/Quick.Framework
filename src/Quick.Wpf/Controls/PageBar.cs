using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    [TemplatePart(Name = ElementFirstPageButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementPrevPageButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementNextPageButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementLastPageButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementGoPageButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementCurPageTextBox, Type = typeof(System.Windows.Controls.TextBox))]
    [TemplatePart(Name = ElementPageInfoTextBlock, Type = typeof(TextBlock))]
    public class PageBar : Control
    {
        #region Constants

        private const string ElementFirstPageButton = "PAR_FirstPageButton";
        private const string ElementPrevPageButton = "PAR_PrevPageButton";
        private const string ElementNextPageButton = "PAR_NextPageButton";
        private const string ElementLastPageButton = "PAR_LastPageButton";
        private const string ElementGoPageButton = "PAR_GoPageButton";
        private const string ElementCurPageTextBox = "PAR_CurPageTextBox";
        private const string ElementPageInfoTextBlock = "PAR_PageInfoTextBlock";
        #endregion Constants

        #region Private members
        private Button _btnFirstPage;
        private Button _btnPrevPage;
        private Button _btnNextPage;
        private Button _btnLastPage;
        private Button _btnGoPage;
        private System.Windows.Controls.TextBox _tbxCurPage;
        private TextBlock _tblPageInfo;
        private bool _isInnerChanged = false;
        #endregion


        public override void OnApplyTemplate()
        {
            _btnFirstPage = (Button)GetTemplateChild(ElementFirstPageButton);
            _btnPrevPage = (Button)GetTemplateChild(ElementPrevPageButton);
            _btnNextPage = (Button)GetTemplateChild(ElementNextPageButton);
            _btnLastPage = (Button)GetTemplateChild(ElementLastPageButton);
            _btnGoPage = (Button)GetTemplateChild(ElementGoPageButton);
            _tbxCurPage = (System.Windows.Controls.TextBox)GetTemplateChild(ElementCurPageTextBox);
            _tblPageInfo = (TextBlock)GetTemplateChild(ElementPageInfoTextBlock);

            _btnFirstPage.Click += btnFirstPage_Click;
            _btnPrevPage.Click += btnPrevPage_Click;
            _btnNextPage.Click += btnNextPage_Click;
            _btnLastPage.Click += btnLastPage_Click;
            _btnGoPage.Click += btnGo_Click;

            Update();
        }

        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register(
            "PageCount", typeof(int), typeof(PageBar), new FrameworkPropertyMetadata(default(int),null));

        public int PageCount
        {
            get => (int)this.GetValue(PageCountProperty);
            private set => this.SetValue(PageCountProperty, value);
        }


        public static readonly DependencyProperty TotalProperty = DependencyProperty.Register(
          "Total", typeof(int), typeof(PageBar), new FrameworkPropertyMetadata(default(int), null));

        public int Total
        {
            get => (int)this.GetValue(TotalProperty);
            private set => this.SetValue(TotalProperty, value);
        }

        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
          "PageIndex", typeof(int), typeof(PageBar),
        new FrameworkPropertyMetadata(default(int), new PropertyChangedCallback(PageIndexPropertyChangedCallback)));

        public static void PageIndexPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as PageBar).UpdateIndex();
        }

        public int PageIndex
        {
            get => (int)this.GetValue(PageIndexProperty);
            set => this.SetValue(PageIndexProperty, value);
        }


        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
          "ItemsSource", typeof(IEnumerable), typeof(PageBar),
        new FrameworkPropertyMetadata(default(IEnumerable), new PropertyChangedCallback(ItemsSourcePropertyChangedCallback)));

        public static void ItemsSourcePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as PageBar).UpdateItemsSource(arg.OldValue, arg.NewValue);
        }
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty DisplayDataProperty = DependencyProperty.Register(
          "DisplayData", typeof(IEnumerable), typeof(PageBar),
        new FrameworkPropertyMetadata(default(IEnumerable), new PropertyChangedCallback(DisplayDataPropertyChangedCallback)));

        public static void DisplayDataPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            //(sender as PageBar).UpdateDisplayData(arg.OldValue, arg.NewValue);
        }

        public IEnumerable DisplayData
        {
            get => (IEnumerable)this.GetValue(DisplayDataProperty);
            set => this.SetValue(DisplayDataProperty, value);
        }


        public static readonly DependencyProperty LimitProperty = DependencyProperty.Register(
              "Limit", typeof(int), typeof(PageBar),
            new FrameworkPropertyMetadata(10, new PropertyChangedCallback(LimitPropertyChangedCallback)));



        public static void LimitPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as PageBar).Update();
        }

        public int Limit
        {
            get => (int)this.GetValue(LimitProperty);
            set => this.SetValue(LimitProperty, value);
        }



        


        public void UpdateIndex()
        {
            if (!_isInnerChanged)
            {
                Update();
            }
        }
        public void UpdateItemsSource(object oldVal, object newVal)
        {
            if (newVal == null)
            {
                DisplayData = null;
                Total = 0;
                PageCount = 0;
                _isInnerChanged = true;
                PageIndex = 0;
                _isInnerChanged = false;
                _tblPageInfo.Text = "0/0";
                return;
            }

            if (DisplayData != null)
            {
                Type displayItemType = DisplayData.GetType().GetGenericArguments()[0];
                if (displayItemType != GetItemType())
                {
                    DisplayData = null;
                }
            }

            Type type = newVal.GetType();
            if (type.HasInterface<INotifyCollectionChanged>())
            {
                (newVal as INotifyCollectionChanged).CollectionChanged += PageBar_CollectionChanged;
            }
            Update();
        }

        private void PageBar_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Update();
        }
        private Type GetItemType()
        {
            return ItemsSource.GetType().GetGenericArguments()[0];
        }
        private void Update()
        {
            if (_tblPageInfo == null)
            {
                return;
            }
            if (ItemsSource == null)
            {
                _tblPageInfo.Text = "0/0";
                return;
            }
            List<object> totalData = ItemsSource.Cast<object>().ToList();
            Total = totalData.Count;
            int pageCount = Total / Limit;
            if (Total % Limit != 0)
            {
                pageCount++;
            }
            PageCount = pageCount;
            _isInnerChanged = true;
            if (PageIndex > pageCount)
            {
                PageIndex = pageCount;
            }
            if (PageCount == 0)
            {
                PageIndex = 0;
            }
            else
            {
                if (PageIndex == 0)
                {
                    PageIndex = 1;
                }
            }
            _isInnerChanged = false;
            List<object> shouldDisplayData = ItemsSource.Cast<object>().Skip((PageIndex - 1) * Limit).Take(Limit).ToList();

            bool hasChanged = DisplayData == null;
            if (!hasChanged)
            {
                List<object> displayData = DisplayData.Cast<object>().ToList();
                if (shouldDisplayData.Count == displayData.Count)
                {
                    for (int i = 0; i < shouldDisplayData.Count; i++)
                    {
                        if (!object.Equals(shouldDisplayData[i], displayData[i]))
                        {
                            hasChanged = true;
                            break;
                        }
                    }
                }
                else
                {
                    hasChanged = true;
                }
            }

            if (hasChanged)
            {
                if (DisplayData == null)
                {
                    IList list = (IList)Activator.CreateInstance(typeof(ObservableCollection<>).MakeGenericType(GetItemType()));
                    foreach (object obj in shouldDisplayData)
                    {
                        list.Add(obj);
                    }
                    DisplayData = list;
                }
                else
                {
                    IList displayList = DisplayData as IList;
                    displayList.Clear();
                    foreach (object obj in shouldDisplayData)
                    {
                        displayList.Add(obj);
                    }
                }
            }
            _tblPageInfo.Text = $"{PageIndex}/{PageCount}";
            _tbxCurPage.Text = PageIndex.ToString();
        }

        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex > 1)
            {
                PageIndex = 1;
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex > 1)
            {
                PageIndex--;
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex < PageCount)
            {
                PageIndex++;
            }
        }

        private void btnLastPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex < PageCount)
            {
                PageIndex = PageCount;
            }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            int pageIndex = PageIndex;
            if (int.TryParse(_tbxCurPage.Text, out pageIndex))
            {
                PageIndex = pageIndex;
            }
            else
            {
                _tbxCurPage.Text = PageIndex.ToString();
            }
        }
    }
}
