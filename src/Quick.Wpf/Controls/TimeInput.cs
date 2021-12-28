using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using Quick;
using System.Text.RegularExpressions;

namespace Quick
{
    [TemplatePart(Name = ElementTbx1, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementTbx2, Type = typeof(TextBox))]
    public partial class TimeInput : Control
    {

        #region Constants

        private const string ElementTbx1 = "PART_TextBox1";
        private const string ElementTbx2 = "PART_TextBox2";

        #endregion

        #region Properties

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
          "Time", typeof(DateTime?), typeof(TimeInput),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TimePropertyChangedCallback)));

        public static void TimePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as TimeInput).UpdateTime();
        }

        [TypeConverter(typeof(XamlStringToTimeConverter))]
        public DateTime? Time
        {
            get
            {
                return (DateTime?)this.GetValue(TimeProperty);
            }
            set
            {
                this.SetValue(TimeProperty, value);
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
          "Text", typeof(string), typeof(TimeInput),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TextPropertyChangedCallback)));

        public static void TextPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as TimeInput).UpdateText();
        }

        [TypeConverter(typeof(XamlStringToTimeConverter))]
        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }
            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
              "IsReadOnly", typeof(bool), typeof(TimeInput),
              new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsReadOnlyPropertyChangedCallback)));

        public static void IsReadOnlyPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as TimeInput).UpdateIsReadOnly();
        }

        public bool IsReadOnly
        {
            get
            {
                return (bool)this.GetValue(IsReadOnlyProperty);
            }
            set
            {
                this.SetValue(IsReadOnlyProperty, value);
            }
        }

        #endregion

        #region Public methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tbx1 = GetTemplateChild(ElementTbx1) as TextBox;
            _tbx2 = GetTemplateChild(ElementTbx2) as TextBox;
            _boxes.Add(_tbx1);
            _boxes.Add(_tbx2);
            foreach (var tbx in _boxes)
            {
                tbx.TextChanged += TextBox_TextChanged;
                tbx.PreviewKeyDown += TextBox_PreviewKeyDown;
            }
            UpdateTime();
        }

        #endregion

        #region Private members

        private bool _isInnerChanged = false;
        private TextBox _tbx1;
        private TextBox _tbx2;
        private List<TextBox> _boxes = new List<TextBox>();

        #endregion

        #region Private methods

        private void UpdateIsReadOnly()
        {
            foreach (TextBox tbx in _boxes)
            {
                tbx.IsReadOnly = IsReadOnly;
            }
        }

        private void UpdateTime()
        {
            if (_isInnerChanged)
            {
                return;
            }
            _isInnerChanged = true;
            if (Time == null)
            {
                foreach (TextBox tbx in _boxes)
                {
                    tbx.Clear();
                }
                Text = null;
            }
            else
            {

                _tbx1.Text = Time.Value.Hour.ToString("D02");
                _tbx2.Text = Time.Value.Minute.ToString("D02");
                Text = $"{_tbx1.Text}:{_tbx2.Text}";
            }
            _isInnerChanged = false;
        }

        private bool IsValidTimeStr(string timeStr)
        {
            return Regex.IsMatch(timeStr, @"^(([0-1]?[\d])|(2[0-3])):[0-5]?\d$");
        }

        private void UpdateText()
        {
            if (_isInnerChanged)
            {
                return;
            }
            _isInnerChanged = true;
            if (Text.IsNullOrWhiteSpace())
            {
                foreach (TextBox tbx in _boxes)
                {
                    tbx.Clear();
                }
                Time = null;
            }
            else if (!IsValidTimeStr(Text))
            {
                foreach (TextBox tbx in _boxes)
                {
                    tbx.Clear();
                }
                Text = null;
                Time = null;
            }
            else
            {
                string[] vals = Text.Split(':');
                _tbx1.Text = vals[0];
                _tbx2.Text = vals[1];
                DateTime dtNow = DateTime.Now;
                Time = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, Convert.ToInt32(vals[0]), Convert.ToInt32(vals[1]), 0);
            }
            _isInnerChanged = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isInnerChanged)
            {
                return;
            }
            TextBox tbx = sender as TextBox;
            int maxVal = tbx == _tbx1 ? 23 : 59;
            if (tbx.Text.Length > 0)
            {
                int val = 0;
                int.TryParse(tbx.Text, out val);

                if (val > maxVal)
                {
                    _isInnerChanged = true;
                    tbx.Text = maxVal.ToString();
                    tbx.Select(2, 0);
                    _isInnerChanged = false;
                }
                else
                {
                    if (tbx.Text.Length >= 2 && tbx == _tbx1)
                    {
                        _tbx2.Focus();
                        if (_tbx2.Text.Length > 0)
                        {
                            _tbx2.SelectAll();
                        }
                        else
                        {
                            _tbx2.Select(0, 0);
                        }
                    }
                }
            }
            byte?[] vals = new byte?[2];
            for (int i = 0; i < _boxes.Count; i++)
            {
                maxVal = _boxes[i] == _tbx1 ? 23 : 59;
                int tempVal = 0;
                if (_boxes[i].Text.Length > 0)
                {
                    int.TryParse(_boxes[i].Text, out tempVal);
                    if (tempVal > maxVal)
                    {
                        tempVal = maxVal;
                        _isInnerChanged = true;
                        _boxes[i].Text = maxVal.ToString();
                        _isInnerChanged = false;
                    }
                    vals[i] = (byte)tempVal;
                }

            }
            _isInnerChanged = true;
            if (vals.Any(p => p == null))
            {

                Time = null;
                Text = null;
            }
            else
            {
                //Address = new IPAddress(vals.Select(p => p.Value).ToArray());
                DateTime dtNow = DateTime.Now;
                DateTime dtTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, vals[0].Value, vals[1].Value, 0);
                Time = dtTime;
                Text = $"{dtTime.Hour:D02}:{dtTime.Minute:D02}";
            }
            _isInnerChanged = false;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            int index = _boxes.IndexOf(tbx);
            if (e.Key == Key.OemPeriod || e.Key == Key.Decimal)
            {
                if (index < 3)
                {
                    _boxes[index + 1].Focus();
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                if (tbx.CaretIndex == 0)
                {
                    if (index > 0)
                    {
                        TextBox prevTbx = _boxes[index - 1];
                        prevTbx.Focus();
                        if (prevTbx.Text.Length > 0)
                        {
                            prevTbx.Select(prevTbx.Text.Length, 0);
                        }
                    }
                }
            }
            else if (e.Key == Key.Left)
            {
                if (tbx.CaretIndex == 0)
                {
                    if (index > 0)
                    {
                        TextBox prevTbx = _boxes[index - 1];
                        prevTbx.Focus();
                        if (prevTbx.Text.Length > 0)
                        {
                            prevTbx.Select(prevTbx.Text.Length, 0);
                        }
                        e.Handled = true;
                    }
                }
            }
            else if (e.Key == Key.Right || e.Key == Key.OemSemicolon)
            {
                if (tbx.CaretIndex == tbx.Text.Length)
                {
                    if (index < 1)
                    {
                        TextBox nextTbx = _boxes[index + 1];
                        nextTbx.Focus();
                        nextTbx.Select(0, 0);
                        e.Handled = true;
                    }
                }
                if (e.Key == Key.OemSemicolon)
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (e.Key == Key.Home
                  || e.Key == Key.End
                  || (e.Key >= Key.D0 && e.Key <= Key.D9)
                  || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                  )
                {
                    ;
                }
                else
                {
                    e.Handled = true;
                }
                return;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            if (tbx.Text.Length == 1)
            {
                _isInnerChanged = true;
                tbx.Text = tbx.Text.PadLeft(2, '0');
                _isInnerChanged = false;
            }
        }

        #endregion
    }

    public class XamlStringToTimeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
                                            object value)
        {
            try
            {
                if (value is string)
                {
                    return StringToTimeConverter.StringToTime((string)value);
                }
                return base.ConvertFrom(context, culture, value);
            }
            catch
            {
                return null;
            }
        }
    }
}
