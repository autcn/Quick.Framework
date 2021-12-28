using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Quick;

namespace Quick
{
    public abstract class QEditCreatorBase<TAttribute> : IQEditControlCreator<TAttribute> where TAttribute : QEditAttribute, new()
    {

        #region Abstract methods
        public abstract FrameworkElement CreateElement(QEditContext<TAttribute> qEditContext);

        /// <summary>
        /// 默认的TextColumn，TextBlock + TextBox
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="qEditContext"></param>
        /// <returns></returns>
        public virtual DataGridColumn CreateDataGridColumn(DataGrid dataGrid, QEditContext<TAttribute> qEditContext)
        {
            DataGridTextColumn textCol = new DataGridTextColumn();
            QEditAttribute attr = qEditContext.Attr;

            //编辑框样式，限制数字等
            //if (attr is QTextBoxAttribute
            //   || attr is QSliderAttribute)
            //{
            SetDataGridColumnTextBoxStyle(dataGrid, textCol, qEditContext);
            //}

            //列只读
            textCol.IsReadOnly = qEditContext.Attr.IsReadOnly || !qEditContext.Attr.IsEnabled;

            //绑定
            Binding binding = new Binding(qEditContext.PropertyName);
            binding.Mode = textCol.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            if (qEditContext.PropertyType == typeof(string))
            {
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            }
            else
            {
                binding.UpdateSourceTrigger = UpdateSourceTrigger.Default;
            }
            textCol.Binding = binding;

            //TextBlock样式，主要是对齐
            SetDataGridColumnTextBlockStyle(dataGrid, textCol, qEditContext);

            return textCol;
        }
        #endregion



        #region Protected methods

        #region DataGrid
        protected void SetDataGridColumnTextBlockStyle(DataGrid dataGrid, DataGridTextColumn textCol, QEditContext<TAttribute> qEditContext)
        {
            var attr = qEditContext.Attr;
            if (attr.DataGridColumnAlignment == TextAlignment.Left)
            {
                textCol.ElementStyle = (Style)dataGrid.FindResource(StyleKeysProperties.QDataGridCellLeftStyleKey);
            }
            else if (attr.DataGridColumnAlignment == TextAlignment.Right)
            {
                textCol.ElementStyle = (Style)dataGrid.FindResource(StyleKeysProperties.QDataGridCellRightStyleKey);
            }
            else
            {
                textCol.ElementStyle = (Style)dataGrid.FindResource(StyleKeysProperties.QDataGridCellCenterStyleKey);
            }
        }

        protected void SetDataGridColumnTextBlockStyle(DataGrid dataGrid, FrameworkElementFactory textblockFactory, QEditContext<TAttribute> qEditContext)
        {
            if (textblockFactory.Type != typeof(TextBlock))
            {
                throw new Exception("The type of the factory must be TextBlock");
            }
            var attr = qEditContext.Attr;
            if (attr.DataGridColumnAlignment == TextAlignment.Left)
            {
                textblockFactory.SetValue(TextBlock.StyleProperty, (Style)dataGrid.FindResource(StyleKeysProperties.QDataGridCellLeftStyleKey));
            }
            else if (attr.DataGridColumnAlignment == TextAlignment.Right)
            {
                textblockFactory.SetValue(TextBlock.StyleProperty, (Style)dataGrid.FindResource(StyleKeysProperties.QDataGridCellRightStyleKey));
            }
            else
            {
                textblockFactory.SetValue(TextBlock.StyleProperty, (Style)dataGrid.FindResource(StyleKeysProperties.QDataGridCellCenterStyleKey));
            }
        }

        private void SetDataGridColumnTextBoxStyle(DataGrid dataGrid, DataGridTextColumn textCol, QEditContext<TAttribute> qEditContext)
        {
            var attr = qEditContext.Attr;
            Type realType = qEditContext.PropertyType.GetNullableUnderlyingType();
            Style textBoxStyle = new Style();
            textBoxStyle.TargetType = typeof(TextBox);
            Style baseStyle = (Style)dataGrid.FindResource(typeof(TextBox));
            if (baseStyle != null)
                textBoxStyle.BasedOn = baseStyle;
            if (CSharpTypeCategory.IntegerTypes.Contains(realType))
            {
                bool allowNegative = !realType.Name.ToLower().StartsWith("u");
                int maxLength = int.MaxValue;
                if (CSharpTypeCategory.IntegerTypes32.Contains(realType))
                {
                    maxLength = int.MaxValue.ToString().Length;
                }
                else if (CSharpTypeCategory.IntegerTypes16.Contains(realType))
                {
                    maxLength = short.MaxValue.ToString().Length;
                }
                else if (CSharpTypeCategory.IntegerTypes64.Contains(realType))
                {
                    maxLength = long.MaxValue.ToString().Length;
                }
                else if (CSharpTypeCategory.IntegerTypes8.Contains(realType))
                {
                    maxLength = byte.MaxValue.ToString().Length;
                }
                InputChars intputTypes = InputChars.Number;
                if (allowNegative)
                {
                    intputTypes |= InputChars.Negative;
                }

                textBoxStyle.Setters.Add(new Setter(TextBox.MaxLengthProperty, maxLength));
                textBoxStyle.Setters.Add(new Setter(TextBoxHelper.InputCharsProperty, intputTypes));
                textBoxStyle.Setters.Add(new Setter(InputMethod.IsInputMethodEnabledProperty, false));
            }
            else if (CSharpTypeCategory.FloatTypes.Contains(realType))
            {
                textBoxStyle.Setters.Add(new Setter(TextBoxHelper.InputCharsProperty, InputCharsModels.NegativeFloat));
                textBoxStyle.Setters.Add(new Setter(InputMethod.IsInputMethodEnabledProperty, false));
            }
            else if (realType == typeof(string))
            {
                if (attr is QTextBoxAttribute textBoxAttr)
                {
                    if (!textBoxAttr.InputChars.HasFlag(InputChars.All))
                    {
                        textBoxStyle.Setters.Add(new Setter(TextBoxHelper.InputCharsProperty, textBoxAttr.InputChars));
                    }
                }
            }

            textBoxStyle.Setters.Add(new Setter(TextBox.HorizontalContentAlignmentProperty, TextAligmentToHAliment(attr.DataGridColumnAlignment)));
            textBoxStyle.Setters.Add(new Setter(TextBox.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            textCol.EditingElementStyle = textBoxStyle;
        }

        protected HorizontalAlignment TextAligmentToHAliment(TextAlignment alignment)
        {
            if (alignment == TextAlignment.Left)
            {
                return HorizontalAlignment.Left;
            }

            if (alignment == TextAlignment.Center)
            {
                return HorizontalAlignment.Center;
            }

            if (alignment == TextAlignment.Right)
            {
                return HorizontalAlignment.Right;
            }

            return HorizontalAlignment.Center;
        }

        protected void SetReadOnlyDataGridColumnBinding(DataGridBoundColumn column, QEditContext<TAttribute> qEditContext, Type toConvertType = null, IValueConverter valConverter = null, string stringFormat = null)
        {
            Binding binding = new Binding(qEditContext.PropertyName);
            binding.Mode = BindingMode.OneWay;
            if (toConvertType != null && valConverter != null && qEditContext.PropertyType.GetNullableUnderlyingType() == toConvertType)
            {
                binding.Converter = valConverter;
            }
            if (!stringFormat.IsNullOrEmpty())
            {
                binding.StringFormat = stringFormat;
            }
            column.Binding = binding;
        }
        protected DataTemplate CreateTextBlockTemplate(DataGrid dataGrid, QEditContext<TAttribute> qEditContext, Type toConvertType = null, IValueConverter valConverter = null, string stringFormat = null)
        {
            DataTemplate textTemplate = new DataTemplate();
            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.TextAlignmentProperty, qEditContext.Attr.DataGridColumnAlignment);
            Binding textBinding = new Binding(qEditContext.PropertyName);
            textBinding.Mode = BindingMode.OneWay;
            if (toConvertType != null && valConverter != null && qEditContext.PropertyType.GetNullableUnderlyingType() == toConvertType)
            {
                textBinding.Converter = valConverter;
            }
            if (!stringFormat.IsNullOrEmpty())
            {
                textBinding.StringFormat = stringFormat;
            }
            textBlock.SetBinding(TextBlock.TextProperty, textBinding);
            SetDataGridColumnTextBlockStyle(dataGrid, textBlock, qEditContext);
            textTemplate.VisualTree = textBlock;
            return textTemplate;
        }

        protected DataTemplate CreateEditTemplate(QEditContext<TAttribute> qEditContext, Type controlType, DependencyProperty bindingProperty,
                    Type toConvertType = null, IValueConverter valConverter = null)
        {
            DataTemplate editTemplate = new DataTemplate();
            var control = new FrameworkElementFactory(controlType);
            Binding editBinding = CreateBinding(qEditContext);
            if (qEditContext.PropertyType == toConvertType)
            {
                editBinding.Converter = valConverter;
            }
            control.SetBinding(bindingProperty, editBinding);
            editTemplate.VisualTree = control;
            return editTemplate;
        }
        #endregion

        #region Edit Wnd
        protected Binding CreateBinding(QEditContext<TAttribute> qEditContext, bool usePropertyChangedTrigger = false)
        {
            Type realType = qEditContext.PropertyType.GetNullableUnderlyingType();
            Binding binding = new Binding(qEditContext.PropertyName);
            binding.Mode = qEditContext.Attr.BindingMode;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.Default;
            binding.ValidatesOnDataErrors = true;
            if (!qEditContext.Attr.StringFormat.IsNullOrEmpty())
            {
                binding.StringFormat = qEditContext.Attr.StringFormat;
            }
            if (qEditContext.Attr.BindingMode == BindingMode.Default)
            {
                binding.Mode = BindingMode.TwoWay;
            }

            if (realType == typeof(double) || realType == typeof(float) || realType == typeof(decimal))
            {
                binding.Mode = BindingMode.Default;
            }

            if (realType == typeof(string) || realType == typeof(DateTime) || usePropertyChangedTrigger)
            {
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            }

            return binding;
        }
        #endregion

        #endregion
    }
}
