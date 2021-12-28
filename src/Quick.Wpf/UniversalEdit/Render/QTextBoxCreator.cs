
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Quick
{
    public class QTextBoxCreator : QEditCreatorBase<QTextBoxAttribute>
    {
        public override FrameworkElement CreateElement(QEditContext<QTextBoxAttribute> qEditContext)
        {
            QTextBoxAttribute attr = qEditContext.Attr;
            TextBox tbx = new TextBox();

            Binding binding = CreateBinding(qEditContext, true);
            if (qEditContext.PropertyType.IsNullable())
            {
                binding.TargetNullValue = String.Empty;
            }
            Type realType = qEditContext.PropertyType.GetNullableUnderlyingType();
            if (CSharpTypeCategory.FloatTypes.Contains(realType))
            {
                binding.Delay = 300;
            }
            tbx.SetBinding(TextBox.TextProperty, binding);
            tbx.MaxLength = attr.MaxLength;
            tbx.IsReadOnly = attr.IsReadOnly;
            //tbx.ShowClearButton = true;
            string normalText = attr.WaterMark.StrongTextToNormal();
            if (attr.WaterMark.IsResourceKey())
            {
                //tbx.SetResourceReference(InfoElement.PlaceholderProperty, normalText);
            }
            else if(attr.WaterMark != null)
            {
                //InfoElement.SetPlaceholder(tbx, normalText);
            }
            SetTextBoxLimit(tbx, qEditContext);
            tbx.HorizontalContentAlignment = qEditContext.Attr.Alignment;
            return tbx;
        }

        protected virtual void SetTextBoxLimit(TextBox tbx, QEditContext<QTextBoxAttribute> qEditContext)
        {
            QTextBoxAttribute attr = qEditContext.Attr;
            Type realType = qEditContext.PropertyType.GetNullableUnderlyingType();

            if (CSharpTypeCategory.IntegerTypes.Contains(realType))
            {
                bool allowNegative = !realType.Name.ToLower().StartsWith("u");
                if (CSharpTypeCategory.IntegerTypes32.Contains(realType))
                {
                    tbx.MaxLength = int.MaxValue.ToString().Length;
                }
                else if (CSharpTypeCategory.IntegerTypes16.Contains(realType))
                {
                    tbx.MaxLength = short.MaxValue.ToString().Length;
                }
                else if (CSharpTypeCategory.IntegerTypes64.Contains(realType))
                {
                    tbx.MaxLength = long.MaxValue.ToString().Length;
                }
                else if (CSharpTypeCategory.IntegerTypes8.Contains(realType))
                {
                    tbx.MaxLength = byte.MaxValue.ToString().Length;
                }
                InputChars intputTypes = InputChars.Number;
                if (allowNegative)
                {
                    intputTypes |= InputChars.Negative;
                }
                TextBoxHelper.SetInputChars(tbx, intputTypes);
                InputMethod.SetIsInputMethodEnabled(tbx, false);
            }
            else if (CSharpTypeCategory.FloatTypes.Contains(realType))
            {
                TextBoxHelper.SetInputChars(tbx, InputCharsModels.NegativeFloat);
                InputMethod.SetIsInputMethodEnabled(tbx, false);
            }
            else if (realType == typeof(string))
            {
                if (!attr.InputChars.HasFlag(InputChars.All))
                {
                    TextBoxHelper.SetInputChars(tbx, attr.InputChars);
                }
            }
        }
    }
}
