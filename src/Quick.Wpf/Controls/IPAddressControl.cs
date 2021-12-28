using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quick
{
    [TemplatePart(Name = ElementTbx1, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementTbx2, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementTbx3, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementTbx4, Type = typeof(TextBox))]
    public class IPAddressControl : Control
    {
        #region Constants

        private const string ElementTbx1 = "PART_TextBox1";
        private const string ElementTbx2 = "PART_TextBox2";
        private const string ElementTbx3 = "PART_TextBox3";
        private const string ElementTbx4 = "PART_TextBox4";

        #endregion Constants

        #region Properties

        public static readonly DependencyProperty AddressProperty = DependencyProperty.Register(
         "Address", typeof(IPAddress), typeof(IPAddressControl),
         new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(AddressPropertyChangedCallback)));

        public static void AddressPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as IPAddressControl).UpdateAddress();
        }

        [TypeConverter(typeof(IPAddressConverter))]
        public IPAddress Address
        {
            get
            {
                return (IPAddress)this.GetValue(AddressProperty);
            }
            set
            {
                this.SetValue(AddressProperty, value);
            }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = TextBox.IsReadOnlyProperty.AddOwner(typeof(IPAddressControl));

        
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

        #region Private members
        private bool _isInnerChanged = false;
        private TextBox _tbx1;
        private TextBox _tbx2;
        private TextBox _tbx3;
        private TextBox _tbx4;
        private List<TextBox> _boxes = new List<TextBox>();
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tbx1 = GetTemplateChild(ElementTbx1) as TextBox;
            _tbx2 = GetTemplateChild(ElementTbx2) as TextBox;
            _tbx3 = GetTemplateChild(ElementTbx3) as TextBox;
            _tbx4 = GetTemplateChild(ElementTbx4) as TextBox;
            _boxes.Add(_tbx1);
            _boxes.Add(_tbx2);
            _boxes.Add(_tbx3);
            _boxes.Add(_tbx4);
            foreach (var tbx in _boxes)
            {
                tbx.TextChanged += Tbx_TextChanged;
                tbx.PreviewKeyDown += Tbx_PreviewKeyDown;
            }
            UpdateAddress();
        }

        private void Tbx_PreviewKeyDown(object sender, KeyEventArgs e)
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
                        var prevTbx = _boxes[index - 1];
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
                        var prevTbx = _boxes[index - 1];
                        prevTbx.Focus();
                        if (prevTbx.Text.Length > 0)
                        {
                            prevTbx.Select(prevTbx.Text.Length, 0);
                        }
                        e.Handled = true;
                    }
                }
            }
            else if (e.Key == Key.Right)
            {
                if (tbx.CaretIndex == tbx.Text.Length)
                {
                    if (index < 3)
                    {
                        var nextTbx = _boxes[index + 1];
                        nextTbx.Focus();
                        nextTbx.Select(0, 0);
                        e.Handled = true;
                    }
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
   

        private void Tbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isInnerChanged)
            {
                return;
            }
            TextBox tbx = sender as TextBox;
            if (tbx.Text.Length > 0)
            {
                int val = 0;
                int.TryParse(tbx.Text, out val);
                if (val > 255)
                {
                    _isInnerChanged = true;
                    tbx.Text = "255";
                    tbx.Select(3, 0);
                    _isInnerChanged = false;
                }
            }
            byte?[] vals = new byte?[4];
            for (int i = 0; i < _boxes.Count; i++)
            {
                int tempVal = 0;
                if (_boxes[i].Text.Length > 0)
                {
                    int.TryParse(_boxes[i].Text, out tempVal);
                    if (tempVal > 255)
                    {
                        tempVal = 255;
                        _isInnerChanged = true;
                        _boxes[i].Text = "255";
                        _isInnerChanged = false;
                    }
                    vals[i] = (byte)tempVal;
                }

            }
            _isInnerChanged = true;
            if (vals.Any(p => p == null))
            {

                Address = null;

            }
            else
            {
                Address = new IPAddress(vals.Select(p => p.Value).ToArray());
            }
            _isInnerChanged = false;
        }

        private void UpdateAddress()
        {
            if (_isInnerChanged)
            {
                return;
            }
            if(_boxes.Count == 0 )
            {
                return;
            }
            if (Address == null)
            {
                foreach (var tbx in _boxes)
                {
                    tbx.Clear();
                }
            }
            else
            {
                byte[] vals = Address.GetAddressBytes();
                for (int i = 0; i < 4; i++)
                {
                    if (vals[i] == 0 && _boxes[i].Text == "")
                    {
                        _boxes[i].Text = "0";
                    }
                    else
                    {
                        _boxes[i].Text = vals[i].ToString();
                    }
                }
            }
        }
    }
}

