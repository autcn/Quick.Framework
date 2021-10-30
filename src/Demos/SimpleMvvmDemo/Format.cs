using Quick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMvvmDemo
{
    public class Format : IFormat
    {
        public string ToDataSizeDes(long size)
        {
            double fSize = (double)size / 1024.0;
            string unit = "KB";
            if (fSize >= 1024)
            {
                fSize /= 1024.0;
                unit = "MB";
            }

            if (fSize >= 1024)
            {
                fSize /= 1024.0;
                unit = "GB";
            }
            return fSize.ToString("F02") + " " + unit;
        }
    }
}
