using Quick;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Quick
{
    /// <summary>
    /// SpinnerControl.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = ElementDeButton, Type = typeof(Button))]
    [TemplatePart(Name = ElemenNumberTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementInButton, Type = typeof(Button))]
    public class SpinnerControl : Control
    {
        #region Constants

        private const string ElementDeButton = "PART_DecreaseButton";
        private const string ElemenNumberTextBox = "PART_NumberTextBox";
        private const string ElementInButton = "PART_IncreaseButton";

        #endregion Constants

        #region Properties

        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
              "Number", typeof(int), typeof(SpinnerControl),
              new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(NumberPropertyChangedCallback)));

        public static void NumberPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as SpinnerControl).UpdateText();
        }

        public int Number
        {
            get
            {
                return (int)this.GetValue(NumberProperty);
            }
            set
            {
                this.SetValue(NumberProperty, value);
            }
        }

        public static readonly DependencyProperty MinNumberProperty =
        DependencyProperty.Register("MinNumber", typeof(int), typeof(SpinnerControl), new FrameworkPropertyMetadata(0, OnMinNumberChanged));

        public int MinNumber
        {
            get
            {
                return (int)this.GetValue(MinNumberProperty);
            }
            set
            {
                this.SetValue(MinNumberProperty, value);
            }
        }

        private static void OnMinNumberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as SpinnerControl).UpdateText();
        }


        public static readonly DependencyProperty MaxNumberProperty = DependencyProperty.Register(
                  "MaxNumber", typeof(int), typeof(SpinnerControl),
                  new FrameworkPropertyMetadata(int.MaxValue, new PropertyChangedCallback(MaxNumberPropertyChangedCallback)));

        public static void MaxNumberPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as SpinnerControl).UpdateText();
        }

        public int MaxNumber
        {
            get
            {
                return (int)this.GetValue(MaxNumberProperty);
            }
            set
            {
                this.SetValue(MaxNumberProperty, value);
            }
        }

        public static readonly DependencyProperty DecreaseContentProperty = DependencyProperty.Register(
          "DecreaseContent", typeof(object), typeof(SpinnerControl),
        new FrameworkPropertyMetadata(default(object)));


        public object DecreaseContent
        {
            get => (object)this.GetValue(DecreaseContentProperty);
            set => this.SetValue(DecreaseContentProperty, value);
        }


        public static readonly DependencyProperty IncreaseContentProperty = DependencyProperty.Register(
          "IncreaseContent", typeof(object), typeof(SpinnerControl),
        new FrameworkPropertyMetadata(default(object)));


        public object IncreaseContent
        {
            get => (object)this.GetValue(IncreaseContentProperty);
            set => this.SetValue(IncreaseContentProperty, value);
        }

        #endregion

        #region Events

        public event EventHandler<ClickEventArgs> Click;

        #endregion

        #region Private members
        private RepeatButton _btnDecrease;
        private TextBox _tbxNumber;
        private RepeatButton _btnIncrease;
        private bool _isInnerChanged = false;
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _btnDecrease = GetTemplateChild(ElementDeButton) as RepeatButton;
            _tbxNumber = GetTemplateChild(ElemenNumberTextBox) as TextBox;
            _btnIncrease = GetTemplateChild(ElementInButton) as RepeatButton;

            _btnDecrease.Click += (s, e) => ChangeNumber(false);
            _btnIncrease.Click += (s, e) => ChangeNumber(true);

            _tbxNumber.PreviewKeyDown += _tbxNumber_PreviewKeyDown;
            _tbxNumber.TextChanged += _tbxNumber_TextChanged;

            _tbxNumber.Text = Number.ToString();

            UpdateText();
        }


        private void _tbxNumber_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            InputChars chars = InputChars.Number;
            if (MinNumber < 0)
            {
                chars |= InputChars.Negative;
            }
            TextBoxHelper.LimitInputChars(sender, e, chars);
        }


        private void _tbxNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strText = _tbxNumber.Text.Trim();
            if (strText == "")
            {
                _isInnerChanged = true;
                Number = MinNumber;
                _isInnerChanged = false;
            }
            else
            {
                if (int.TryParse(strText, out int a))
                {
                    if (a > MaxNumber)
                    {
                        Number = MaxNumber;
                    }
                    else
                    {
                        Number = a;
                    }
                }
                else
                {
                    Number = MinNumber;
                }
            }
            _tbxNumber.Select(_tbxNumber.Text.Length, 0);
        }

        private void ChangeNumber(bool isIncrease)
        {
            if (isIncrease)
            {
                if (Number + 1 <= MaxNumber)
                {
                    Number++;
                    Click?.Invoke(this, new ClickEventArgs(isIncrease));
                }
            }
            else
            {
                if (Number > MinNumber)
                {
                    Number--;
                    Click?.Invoke(this, new ClickEventArgs(isIncrease));
                }
            }
        }

        private void UpdateText()
        {
            if (_isInnerChanged)
            {
                return;
            }
            if (Number < MinNumber)
            {
                Number = MinNumber;
            }
            else if (Number > MaxNumber)
            {
                Number = MaxNumber;
            }
            if (_tbxNumber != null)
            {
                _tbxNumber.Text = Number.ToString();
                _tbxNumber.MaxLength = MaxNumber.ToString().Length;
            }
        }

    }

    public class ClickEventArgs
    {
        public ClickEventArgs(bool isIncrease)
        {
            IsIncrease = isIncrease;
        }
        public bool IsIncrease { get; }
    }

}
