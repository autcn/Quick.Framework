using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quick
{
    [Flags]
    public enum InputChars
    {
        All = 1,
        Number = 2,
        Negative = 4,
        MultiSymbols = 8,
        Dot = 16,
        Colon = 32
    }

    public static class TextBoxHelper //: DependencyObject
    {
        public static readonly DependencyProperty InputCharsProperty =
                  DependencyProperty.RegisterAttached("InputChars", typeof(InputChars), typeof(TextBoxHelper), new PropertyMetadata(InputChars.All, OnInputCharsChanged));

        public static InputChars GetInputTypes(DependencyObject obj)
        {
            return (InputChars)obj.GetValue(InputCharsProperty);
        }

        public static void SetInputChars(DependencyObject obj, InputChars value)
        {
            obj.SetValue(InputCharsProperty, value);
        }

        private static void OnInputCharsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TextBox tbx = obj as TextBox;
            if (tbx == null)
            {
                return;
            }

            if (((InputChars)e.OldValue).HasFlag(InputChars.All))
            {
                tbx.PreviewKeyDown += Fe_PreviewKeyDown;
                InputMethod.SetIsInputMethodEnabled(tbx, false);
            }
            else if (((InputChars)e.NewValue).HasFlag(InputChars.All))
            {
                tbx.PreviewKeyDown -= Fe_PreviewKeyDown;
                InputMethod.SetIsInputMethodEnabled(tbx, true);
            }
        }

        private static void Fe_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            InputChars types = GetInputTypes(tbx);
            LimitInputChars(sender, e, types);
        }

        public static void LimitInputChars(object sender, KeyEventArgs e, InputChars inputChars)
        {
            //不限制
            if (inputChars.HasFlag(InputChars.All))
            {
                return;
            }
            Key key = e.Key;
            //常规允许
            if (
                  (key >= Key.D0 && key <= Key.D9)
               || (key >= Key.NumPad0 && key <= Key.NumPad9)
               || (key == Key.Back || key == Key.Escape || key == Key.Tab || key == Key.Left || key == Key.Right || key == Key.Enter
               || key == Key.Home || key == Key.End)
             )
            {
                return;
            }

            TextBox tb = sender as TextBox;

            //允许点
            if (inputChars.HasFlag(InputChars.Dot))
            {
                //允许多点
                if (inputChars.HasFlag(InputChars.MultiSymbols))
                {
                    if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
                    {
                        return;
                    }
                }
                else //不允许多点
                {
                    if ((e.Key == Key.Decimal || e.Key == Key.OemPeriod) && !tb.Text.Contains("."))
                    {
                        return;
                    }
                }
            }

            //允许冒号
            if (inputChars.HasFlag(InputChars.Colon))
            {
                //允许多冒号
                if (inputChars.HasFlag(InputChars.MultiSymbols))
                {
                    if (e.Key == Key.OemSemicolon && (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) != 0)
                    {
                        return;
                    }
                }
                else
                {
                    if (!tb.Text.Contains(":") && e.Key == Key.OemSemicolon && (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) != 0)
                    {
                        return;
                    }
                }
            }

            //允许负号
            if (inputChars.HasFlag(InputChars.Negative))
            {
                if (tb.CaretIndex == 0 && tb.Text.IndexOf('-') == -1 && (e.Key == Key.Subtract || e.Key == Key.OemMinus))
                {
                    return;
                }
            }

            e.Handled = true;
        }
    }
}
