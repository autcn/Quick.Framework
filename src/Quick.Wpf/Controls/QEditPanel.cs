using Quick;
using System.Collections.Generic;

namespace System.Windows.Controls
{

    public class QEditPanel : Grid
    {
        #region  Private fields
        private Dictionary<string, QEditContext> _editContextDict;
        #endregion

        #region Constructor
        public QEditPanel()
        {
            this.DataContextChanged += UserControl_DataContextChanged;
        }
        #endregion

        #region Properties
        public ControlTemplate ErrorTemplate { get; set; }
        #endregion

        private void RenderUI()
        {
            foreach (QEditContext qEditContext in _editContextDict.Values)
            {
                QEditAttribute editAttr = qEditContext.GetAttr();
                FrameworkElement fe = (FrameworkElement)this.FindName(qEditContext.PropertyName);
                if (fe == null || !(fe is QElement))
                {
                    continue;
                }

                //获取渲染器
                IQEditControlsRender generator = QServiceProvider.GetService<IQEditControlsRender>();

                //执行渲染
                var renderResult = generator.Render(qEditContext);

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

                //添加Layout控件
                if (renderResult.RemarkTbk != null)
                {
                    if (!string.IsNullOrWhiteSpace(editAttr.RemarkStyleKey))
                    {
                        renderResult.RemarkTbk.Style = GetStyle(editAttr.RemarkStyleKey);
                    }
                }
                QElement qGrid = fe as QElement;
                qGrid.SetContent(renderResult.BodyLayoutElement, renderResult.InputElement);
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
        private void ClearAll()
        {
            List<QElement> grids = WpfHelper.VisualTreeSearchDownGroup<QElement>(this);
            foreach (QElement grd in grids)
            {
                grd.Children.Clear();
            }
        }
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null || !e.NewValue.GetType().IsClass)
            {
                ClearAll();
                return;
            }
            _editContextDict = QEditContextCache.GetTypeEditContextDict(e.NewValue.GetType());
            if (_editContextDict.Count == 0)
            {
                ClearAll();
                return;
            }
            RenderUI();
        }
    }
}
