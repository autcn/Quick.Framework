using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace Quick
{
    internal class QTemplateLoader
    {
        private string _templateBasePath;
        public QTemplateLoader()
        {

        }
        public QTemplateLoader(string templateBasePath)
        {
            _templateBasePath = templateBasePath;
        }
        public string GetXamlTemplateStringByName(string name)
        {
            //string uriString = string.Format("pack://application:,,,/QuickFramework;component/UniversalEditTemplate/{0}.xaml", name);
            string uriString = string.Format(_templateBasePath, name);
            return GetXamlTemplateStringByUri(uriString);

        }
        public string GetXamlTemplateStringByUri(string uriString, UriKind uriKind = UriKind.Absolute)
        {
            Uri uri = new Uri(uriString, uriKind);
            byte[] xamlBytes = WpfHelper.GetResourceFileContent(uri);
            return Encoding.UTF8.GetString(xamlBytes);

        }
        public virtual FrameworkElement Load(string xamlString)
        {
            return Load<FrameworkElement>(xamlString);
        }

        public virtual TElement Load<TElement>(string xamlString) where TElement : FrameworkElement
        {
            ParserContext parserCtx = new ParserContext();
            parserCtx.XmlnsDictionary.Add("", @"http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            parserCtx.XmlnsDictionary.Add("x", @"http://schemas.microsoft.com/winfx/2006/xaml");
            parserCtx.XmlnsDictionary.Add("qk", @"clr-namespace:Quick;assembly=Quick");
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xamlString));
            TElement fe = (TElement)XamlReader.Load(ms, parserCtx);
            return fe;
        }
    }
}
