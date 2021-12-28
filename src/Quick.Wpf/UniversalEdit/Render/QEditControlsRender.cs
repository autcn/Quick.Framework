using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Quick
{

    [SingletonDependency]
    public class QEditControlsRender : IQEditControlsRender
    {
        private readonly IServiceProvider _serviceProvider;
        public QEditControlsRender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected virtual void CreateTitle(QEditContext qEditContext, QEditRenderControlsResult result)
        {
            QEditAttribute editAttr = qEditContext.GetAttr();
            //创建容器
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            //创建星号TextBlock
            TextBlock reqTbk = new TextBlock();
            reqTbk.FontSize = 14;
            reqTbk.Foreground = Brushes.Red;
            reqTbk.FontFamily = new FontFamily("Tahoma");
            reqTbk.Margin = new Thickness(0, 4, 1, 0);
            reqTbk.FontWeight = FontWeights.Bold;
            reqTbk.Text = "*";
            reqTbk.Padding = new Thickness(0);
            reqTbk.HorizontalAlignment = HorizontalAlignment.Center;
            reqTbk.VerticalAlignment = VerticalAlignment.Center;

            //计算是否可见
            PropertyInfo pi = qEditContext.ModelType.GetProperty(qEditContext.PropertyName);
            reqTbk.Visibility = pi.IsDefined(typeof(RequiredAttribute)) ? Visibility.Visible : Visibility.Hidden;

            //创建Title

            UserControl titleTbk = new UserControl();
            titleTbk.VerticalAlignment = VerticalAlignment.Center;

            UserControl colonTbk = new UserControl();
            colonTbk.VerticalAlignment = VerticalAlignment.Center;
            colonTbk.Margin = new Thickness(2, 0, 4, 0);
            colonTbk.SetResourceReference(UserControl.ContentProperty, QLocalizationProperties.StQEditWndColon.StrongTextToNormal());

            StackPanel titlePanel = new StackPanel();
            titlePanel.Orientation = Orientation.Horizontal;
            titlePanel.VerticalAlignment = VerticalAlignment.Center;
            titlePanel.Children.Add(titleTbk);
            titlePanel.Children.Add(colonTbk);
            Grid.SetColumn(titlePanel, 1);

            //资源绑定
            string normalText = editAttr.Title.StrongTextToNormal();
            if (editAttr.Title == null)
            {
                titleTbk.Content = qEditContext.PropertyName;
                //AdjustTitleBlockName(titleTbk);
            }
            else if (editAttr.Title.IsResourceKey())
            {
                titleTbk.SetResourceReference(UserControl.ContentProperty, normalText);
            }
            else
            {
                titleTbk.Content = normalText;
                //AdjustTitleBlockName(titleTbk);
            }

            grid.Children.Add(reqTbk);
            grid.Children.Add(titlePanel);

            result.TitleLayoutElement = grid;
            result.TitleTbk = titleTbk;
            result.RequiredMarkTbk = reqTbk;
        }

        //private void AdjustTitleBlockName(TextBlock tbk)
        //{
        //    if (tbk.Text.IsNullOrEmpty())
        //    {
        //        return;
        //    }
        //    if (!tbk.Text.EndsWith(":") && !tbk.Text.EndsWith("："))
        //    {
        //        tbk.Text += "：";
        //    }
        //}

        private void SetBodyGridWidth(Grid grid, QEditContext qEditContext)
        {
            GridLength colWidth1 = new GridLength(1, GridUnitType.Star); ;
            GridLength colWidth2 = GridLength.Auto;
            QEditAttribute editAttr = qEditContext.GetAttr();
            do
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(editAttr.Width))
                    {
                        if (editAttr.Width.EndsWith("*"))
                        {
                            string strRate = editAttr.Width.Substring(0, editAttr.Width.Length - 1);
                            double width1 = Convert.ToDouble(strRate);
                            if (width1 >= 1)
                            {
                                break;
                            }

                            double width2 = 1.0 - width1;
                            colWidth1 = new GridLength(width1, GridUnitType.Star);
                            colWidth2 = new GridLength(width2, GridUnitType.Star);

                        }
                        else if (editAttr.Width.ToLower() == "auto")
                        {
                            colWidth1 = new GridLength(1, GridUnitType.Star);
                            colWidth2 = GridLength.Auto;
                        }
                        else
                        {
                            double width1 = Convert.ToDouble(editAttr.Width);
                            if (width1 < 0)
                            {
                                break;
                            }
                            colWidth1 = new GridLength(width1, GridUnitType.Pixel);
                            colWidth2 = new GridLength(1, GridUnitType.Star);
                        }
                    }
                }
                catch
                {
                    break;
                }
            } while (false);

            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = colWidth1 });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = colWidth2 });
        }
        private void CreateBody(QEditContext qEditContext, QEditRenderControlsResult result)
        {
            QEditAttribute editAttr = qEditContext.GetAttr();
            //创建容器
            Grid grid = new Grid();
            SetBodyGridWidth(grid, qEditContext);

            //创建Remark UserControl
            UserControl remarkTbk = new UserControl();
            remarkTbk.VerticalAlignment = VerticalAlignment.Center;
            remarkTbk.Margin = new Thickness(4, 0, 0, 0);
            remarkTbk.Padding = new Thickness(0);
            Grid.SetColumn(remarkTbk, 1);


            //资源绑定
            string normalText = editAttr.Remark.StrongTextToNormal();
            if (editAttr.Remark.IsResourceKey())
            {
                remarkTbk.SetResourceReference(UserControl.ContentProperty, normalText);
            }
            else
            {
                remarkTbk.Content = normalText;
            }
            result.InputElement.IsEnabled = editAttr.IsEnabled;

            grid.Children.Add(result.InputElement);
            grid.Children.Add(remarkTbk);

            result.BodyLayoutElement = grid;
            result.RemarkTbk = remarkTbk;
        }
        public QEditRenderControlsResult Render(QEditContext qEditContext)
        {
            QEditAttribute editAttr = qEditContext.GetAttr();
            QEditRenderControlsResult result = new QEditRenderControlsResult();
            //创建标题
            CreateTitle(qEditContext, result);
            if (qEditContext.GetAttr() is QCheckBoxAttribute)
            {
                result.TitleLayoutElement.Visibility = Visibility.Collapsed;
            }
            //创建主输入控件
            Type creatorType = typeof(IQEditControlCreator<>).MakeGenericType(editAttr.GetType());
            object creator = _serviceProvider.GetService(creatorType);
            MethodInfo createFun = creatorType.GetMethod(nameof(IQEditControlCreator<QEditAttribute>.CreateElement));
            FrameworkElement inputElement = (FrameworkElement)createFun.Invoke(creator, new object[] { qEditContext });

            result.InputElement = inputElement;

            //创建Body
            CreateBody(qEditContext, result);
            return result;
        }
    }
}
