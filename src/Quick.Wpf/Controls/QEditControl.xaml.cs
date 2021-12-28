using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    /// <summary>
    /// QEditControl.xaml 的交互逻辑
    /// </summary>
    public partial class QEditControl : UserControl
    {
        #region  Private fields
        private Dictionary<string, QEditContext> _editContextDict;
        private Dictionary<QEditContext, QEditRenderControlsResult> _renderCache = new Dictionary<QEditContext, QEditRenderControlsResult>();
        #endregion

        #region Constructor
        public QEditControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Columns
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
             "Columns", typeof(int), typeof(QEditControl),
             new FrameworkPropertyMetadata(1, new PropertyChangedCallback(ColumnsPropertyChangedCallback)));

        public static void ColumnsPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as QEditControl).UpdateColumns();
        }
        public int Columns
        {
            get
            {
                return (int)this.GetValue(ColumnsProperty);
            }
            set
            {
                this.SetValue(ColumnsProperty, value);
            }
        }
        #endregion

        #region Properties
        public string ColumnWidth { get; set; }
        public GridLength RowHeight { get; set; } = GridLength.Auto;
        public Thickness TitleMargin { get; set; } = new Thickness(8, 2, 0, 2);
        public Thickness InputMargin { get; set; } = new Thickness(4, 2, 0, 2);

        public Style TitleStyle { get; set; }
        public Style RemarkStyle { get; set; }

        public ControlTemplate ErrorTemplate { get; set; }
        #endregion

        #region Create Grid
        private void UpdateColumns()
        {
            if (Columns <= 0)
            {
                return;
            }
            if (spContainer.Children.Count == 0)
            {
                return;
            }
            //spContainer.Children.Clear();
            RenderUI();
        }
        private Grid CreateGrid(int elementCnt)
        {
            List<GridLength> colWidth = ParseColumnWith();
            Grid grdContainer = new Grid();
            for (int i = 0; i < Columns; i++)
            {
                int realColIndex = i * 2;
                grdContainer.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = realColIndex <= colWidth.Count - 1 ? colWidth[realColIndex] : GridLength.Auto
                });
                realColIndex++;
                grdContainer.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = realColIndex <= colWidth.Count - 1 ? colWidth[realColIndex] : new GridLength(1, GridUnitType.Star)
                });
            }
            if (elementCnt > 0)
            {
                int rowCnt = elementCnt / Columns;
                int mod = elementCnt % Columns;
                if (mod > 0)
                {
                    rowCnt++;
                }
                for (int i = 0; i < rowCnt; i++)
                {
                    grdContainer.RowDefinitions.Add(new RowDefinition() { Height = RowHeight });
                }
            }
            return grdContainer;
        }
        private List<GridLength> ParseColumnWith()
        {
            List<GridLength> colWidthList = new List<GridLength>();
            try
            {
                if (!string.IsNullOrWhiteSpace(ColumnWidth))
                {
                    string[] fileds = ColumnWidth.Trim().Trim(',').Replace(" ", "").Split(',');
                    foreach (string val in fileds)
                    {
                        string tempVal = val.Trim().ToLower();
                        if (tempVal == "")
                        {
                            break;
                        }
                        if (tempVal == "auto")
                        {
                            colWidthList.Add(GridLength.Auto);
                        }
                        else if (tempVal.EndsWith("*"))
                        {
                            double starRate = 1.0;
                            if (tempVal.Length > 1)
                            {
                                starRate = Convert.ToDouble(tempVal.Substring(0, tempVal.Length - 1));
                            }
                            colWidthList.Add(new GridLength(starRate, GridUnitType.Star));
                        }
                        else
                        {
                            double pixel = Convert.ToDouble(tempVal);
                            colWidthList.Add(new GridLength(pixel, GridUnitType.Pixel));
                        }
                    }
                }
            }
            catch
            {

            }
            return colWidthList;
        }
        #endregion

        #region Private functions

        private FrameworkElement CreateGroupUI(EditContextGroup group)
        {
            Grid grdContainer = CreateGrid(group.Count);
            int rowIndex = 0;
            int colIndex = 0;
            //获取渲染器
            IQEditControlsRender generator = QServiceProvider.GetService<IQEditControlsRender>();
            foreach (QEditContext qEditContext in group)
            {
                QEditAttribute editAttr = qEditContext.GetAttr();
                if (qEditContext.GetAttr().Place != QEditPlaces.All && editAttr.Place != QEditPlaces.EditControl)
                {
                    continue;
                }

                bool isDataGrid = editAttr is QDataGridAttribute;

                //执行渲染
                QEditRenderControlsResult renderResult = null;
                if (!_renderCache.TryGetValue(qEditContext, out renderResult))
                {
                    renderResult = generator.Render(qEditContext);
                    _renderCache.Add(qEditContext, renderResult);
                }

                if(!isDataGrid)
                {
                    //首先设置专有titleStyle
                    if (!string.IsNullOrWhiteSpace(editAttr.TitleStyleKey))
                    {
                        renderResult.TitleTbk.Style = GetStyle(editAttr.TitleStyleKey);
                    }
                    else if (TitleStyle != null)
                    {
                        renderResult.TitleTbk.Style = TitleStyle;
                    }
                    renderResult.TitleLayoutElement.Margin = TitleMargin;

                    //将Title放入Grid
                    if (renderResult.TitleLayoutElement != null)
                    {
                        Grid.SetColumn(renderResult.TitleLayoutElement, colIndex);
                        Grid.SetRow(renderResult.TitleLayoutElement, rowIndex);
                        grdContainer.Children.Add(renderResult.TitleLayoutElement);
                    }
                    colIndex++;
                }

                
                if(!isDataGrid)
                {
                    //设置输入控件样式
                    if (renderResult.InputElement != null)
                    {
                        Style inputStyle = GetStyle(editAttr.InputStyleKey);
                        if (inputStyle != null)
                        {
                            renderResult.InputElement.Style = inputStyle;
                        }
                        //设置错误模板
                        if (ErrorTemplate != null)
                        {
                            Validation.SetErrorTemplate(renderResult.InputElement, ErrorTemplate);
                        }
                        renderResult.InputElement.IsEnabled = editAttr.IsEnabled;
                    }
                }

                //添加Layout控件
                if (renderResult.BodyLayoutElement != null)
                {
                    if(!isDataGrid)
                    {
                        renderResult.BodyLayoutElement.Margin = InputMargin;
                        Grid.SetRow(renderResult.BodyLayoutElement, rowIndex);
                        Grid.SetColumn(renderResult.BodyLayoutElement, colIndex);
                        grdContainer.Children.Add(renderResult.BodyLayoutElement);
                        if (renderResult.RemarkTbk != null)
                        {
                            if (!string.IsNullOrWhiteSpace(editAttr.RemarkStyleKey))
                            {
                                renderResult.RemarkTbk.Style = GetStyle(editAttr.RemarkStyleKey);
                            }
                            else if (RemarkStyle != null)
                            {
                                renderResult.RemarkTbk.Style = RemarkStyle;
                            }
                        }
                    }
                    else
                    {
                        renderResult.BodyLayoutElement.Margin = InputMargin;
                        Grid.SetRow(renderResult.BodyLayoutElement, rowIndex);
                        Grid.SetColumn(renderResult.BodyLayoutElement, 0);
                        Grid.SetColumnSpan(renderResult.BodyLayoutElement, Columns * 2);
                        grdContainer.Children.Add(renderResult.BodyLayoutElement);
                        colIndex = Columns * 2 - 1;
                    }
                }

                //到达最后一列
                if (colIndex >= Columns * 2 - 1)
                {
                    rowIndex++;
                    colIndex = 0;
                }
                else
                {
                    colIndex++;
                }
            }
            if (group.GroupHeader != null)
            {
                Expander expander = new Expander();
                expander.IsExpanded = true;
                string normalText = group.GroupHeader.StrongTextToNormal();
                if (group.GroupHeader.IsResourceKey())
                {
                    expander.SetResourceReference(Expander.HeaderProperty, normalText);
                }
                else
                {
                    expander.Header = normalText;
                }

                grdContainer.Margin = new Thickness(0, 10, 6, 0);
                expander.Content = grdContainer;
                return expander;
            }
            else
            {
                grdContainer.Margin = new Thickness(0, 10, 6, 0);
            }
            return grdContainer;
        }

        private void RenderUI()
        {
            _renderCache.Clear();
            spContainer.Children.Clear();
            List<EditContextGroup> groups = QEditContextSorter.SortGroup(_editContextDict.Values);
            for (int i = 0; i < groups.Count; i++)
            {
                FrameworkElement fe = CreateGroupUI(groups[i]);
                if (i > 0)
                {
                    fe.Margin = new Thickness(0, 10, 0, 0);
                }
                spContainer.Children.Add(fe);
            }
        }

        private Style GetStyle(string styleKey)
        {
            if (styleKey.IsNullOrEmpty())
            {
                return null;
            }
            styleKey = styleKey.StrongTextToNormal();
            return (Style)this.TryFindResource(styleKey);
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null || !e.NewValue.GetType().IsClass)
            {
                spContainer.Children.Clear();
                _renderCache.Clear();
                return;
            }
            _editContextDict = QEditContextCache.GetTypeEditContextDict(e.NewValue.GetType());
            if (_editContextDict.Count == 0)
            {
                spContainer.Children.Clear();
                _renderCache.Clear();
                return;
            }
            
            RenderUI();
        }
        #endregion

        #region Public functions
        public TElement GetInputElement<TElement>(string propertyName) where TElement : FrameworkElement
        {
            if (!_editContextDict.TryGetValue(propertyName, out QEditContext qEditContext))
            {
                return default(TElement);
            }
            return (TElement)_renderCache[qEditContext].InputElement;
        }

        public FrameworkElement GetInputElement(string propertyName)
        {
            if (!_editContextDict.TryGetValue(propertyName, out QEditContext qEditContext))
            {
                return null;
            }
            return _renderCache[qEditContext].InputElement;
        }

        public async void FocusToFirstEdit()
        {
            await Task.Delay(10);
            foreach (Grid grd in spContainer.Children)
            {
                if (grd != null)
                {
                    if (FocusToGridFirstTextBox(grd))
                    {
                        break;
                    }
                }
            }
        }

        private bool FocusToGridFirstTextBox(Grid grd)
        {
            foreach (UIElement ue in grd.Children)
            {
                if (ue is TextBox tbx)
                {
                    if (tbx.IsEnabled)
                    {
                        tbx.Focus();
                        tbx.SelectAll();
                        return true;
                    }
                }
                else if (ue is Grid innerGrd)
                {
                    if (FocusToGridFirstTextBox(innerGrd))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
