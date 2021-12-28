using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Quick
{
    public enum FitStyles
    {
        None,
        Uniform,
        Fill
    }

    [DesignTimeVisible(true)]
    [TemplatePart(Name = ElementTextBlock, Type = typeof(TextBlock))]
    public class AutoFitTextControl : Control
    {
        #region Constants

        private const string ElementTextBlock = "PAR_TextBlock";

        #endregion Constants

        #region Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                  "Text", typeof(string), typeof(AutoFitTextControl),
                new FrameworkPropertyMetadata(default(string), new PropertyChangedCallback(TextPropertyChangedCallback)));

        public static void TextPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as AutoFitTextControl).UpdateText();
        }

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        public void UpdateText()
        {
            SetScale();
        }
        #endregion

        #region FitStyle
        public static readonly DependencyProperty FitStyleProperty = DependencyProperty.Register(
                 "FitStyle", typeof(FitStyles), typeof(AutoFitTextControl),
               new FrameworkPropertyMetadata(FitStyles.Fill, new PropertyChangedCallback(FontUniformPropertyChangedCallback)));

        public static void FontUniformPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as AutoFitTextControl).UpdateFontUniform();
        }

        public FitStyles FitStyle
        {
            get => (FitStyles)this.GetValue(FitStyleProperty);
            set => this.SetValue(FitStyleProperty, value);
        }

        public void UpdateFontUniform()
        {
            SetScale();
        }
        #endregion

        #region Private members
        private TextBlock _textBlock;

        private ScaleTransform _scale;
        private Size _size = Size.Empty;
        private Size _avaSize = Size.Empty;
        #endregion

        #region override functions
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBlock = GetTemplateChild(ElementTextBlock) as TextBlock;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            SetScale();
        }

        #endregion

        #region Private functions

        private Size GetControlTextSize(string str, Control ctrl)
        {
            return GetFormattedTextRenderSize(str, ctrl.FontFamily, ctrl.FontStyle, ctrl.FontWeight, ctrl.FontStretch, ctrl.FontSize);
        }

        private Size GetFormattedTextRenderSize(string str, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            var formattedText = new FormattedText(
                      str,
                      CultureInfo.CurrentUICulture,
                      FlowDirection.LeftToRight,
                      new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                      fontSize,
                      Brushes.Black
                      );
            Size size = new Size(formattedText.Width, formattedText.Height);
            return size;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _avaSize = constraint;
            double width = _avaSize.Width - (Padding.Left + Padding.Right);
            double height = _avaSize.Height - (Padding.Top + Padding.Bottom);
            _avaSize.Width = Math.Max(0, width);
            _avaSize.Height = Math.Max(0, height);
            Size size = base.MeasureOverride(constraint);
            if (_scale != null && (_scale.ScaleX <= 1.0 || _scale.ScaleY <= 1.0))
            {
                SetScale();
            }
            return size;
        }
        private void SetSize()
        {
            if (string.IsNullOrEmpty(Text))
            {
                _size = Size.Empty;
            }
            else
            {
                _size = GetControlTextSize(Text, this);
            }
        }

        private void SetScale()
        {
            if (FitStyle == FitStyles.None)
            {
                if (_textBlock != null)
                {
                    _textBlock.LayoutTransform = null;
                    _scale = null;
                }
                return;
            }
            SetSize();
            if (_textBlock == null)
            {
                return;
            }
            if (_scale == null)
            {
                _scale = new ScaleTransform();
                _textBlock.LayoutTransform = _scale;
            }
            double scaleX = 1;
            double scaleY = 1;
            if (_size.Width > _avaSize.Width)
            {
                scaleX = _avaSize.Width / _size.Width;
            }
            else
            {
                scaleX = 1;
            }

            if (_size.Height > _avaSize.Height)
            {
                scaleY = _avaSize.Height / _size.Height;
            }
            else
            {
                scaleY = 1;
            }
            if (FitStyle == FitStyles.Uniform)
            {
                double minScale = Math.Min(scaleX, scaleY);
                scaleX = minScale;
                scaleY = minScale;
            }
            _scale.ScaleX = scaleX;
            _scale.ScaleY = scaleY;
        }
        #endregion
    }
}
