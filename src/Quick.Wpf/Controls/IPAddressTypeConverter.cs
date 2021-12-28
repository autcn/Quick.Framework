using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Quick
{
    public class IPAddressConverter : TypeConverter
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
                    return IPAddress.Parse((string)value);
                return base.ConvertFrom(context, culture, value);
            }
            catch
            {
                return null;
            }
        }
    }
}
