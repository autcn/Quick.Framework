using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Xml;

namespace Quick
{
    /* ConverterParameter格式：[默认值][表达式1:转换值][表达式2:转换值][表达式3:转换值]...
     * 默认值：不符合任何一个表达式或转换发生异常时，转换为该值
     * 表达式：支持与(&，注意在xaml里应使用转义字符&amp;代替)，或(|)，非(!)，和小括号，表达式不区分大小写
     * 转换值：转换后的值，前面加@号表示从全局资源中搜索，若找到，取资源的值，找不到则取默认值
     * 绑定例子如下：
     * <Label Content="ABC" Background="{Binding Path=TestProperty, Converter={StaticResource UniversalConverter},ConverterParameter=[Blue][1|2|3:Red][4|5|6:@MyColor]}" />
     * 以下ConverterParameter均为合法：
     * [Blue][1|2|3:Red][4|5|6:Green]   注释：默认为Blue,输入为1，2，3时，输出为Red，输入为4,5,6时，输出为Green
     * [][1|2|3:张三][4|5|6:李四]  注释：默认为""，输入为1，2，3时，输出为张三，输入为4，5，6时，输入为李四
     * [1|2|3:Red][4|5|6:Green] 注释：默认为Red，输入为1，2，3时，输出Red，输入为4，5，6时，输出为Green
     * [@MyColor][1|2|3:Red][4|5|6:Green] 注释：默认为资源MyColor，输入为1，2，3时，输出Red，输入为4，5，6时，输出Green
     * [@MyColor][!(1|2|3):Red] 注释：默认为资源MyColor，输入不为1,2,3时，输出为Red
     */
    public class UniversalConverter : IValueConverter
    {
        public readonly static UniversalConverter Default = new UniversalConverter();
        private object TryGetValueFromResource(string strValueKey)
        {
            if (string.IsNullOrWhiteSpace(strValueKey))
            {
                return strValueKey;
            }
            strValueKey = strValueKey.Trim();
            object value = null;
            if (strValueKey.StartsWith("@"))
            {
                if (strValueKey.Length <= 1)
                {
                    return value;
                }
                strValueKey = strValueKey.TrimStart('@');
                if (System.Windows.Application.Current.Resources.Contains(strValueKey))
                {
                    value = System.Windows.Application.Current.Resources[strValueKey];
                }
                else
                {
                    value = strValueKey;
                }
            }
            else
            {
                value = strValueKey;
            }
            return value;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strParam = parameter as string;
            if (string.IsNullOrEmpty(strParam))
            {
                return value;
            }
            object outDefaultVal = null;
            try
            {
                List<string> lstExpression = new List<string>();
                //先匹配出所有的表达式
                MatchCollection matchList = Regex.Matches(strParam, @"\[(.*?)\]");
                //匹配为空时，不转换，返回value
                if (matchList != null && matchList.Count > 0)
                {
                    //得到所有的表达式
                    foreach (Match m in matchList)
                    {
                        string strExpression = m.Groups[1].Value;
                        lstExpression.Add(strExpression);
                    }

                    //依次判断各表达式
                    foreach (string exp in lstExpression)
                    {
                        string[] exArray = exp.Split(':');
                        //首先必须获取到默认值，第一个合法的值被认为是默认值，非法值则忽略表达式
                        if (outDefaultVal == null)
                        {
                            outDefaultVal = TryGetValueFromResource(exArray[exArray.Length == 1 ? 0 : 1]);
                            if (outDefaultVal == null)
                            {
                                continue;
                            }
                            if (value == null)
                            {
                                return outDefaultVal;
                            }
                        }

                        if (exArray.Length >= 2)
                        {
                            string strInVal = "";
                            // MessageBox.Show(value.ToString());
                            if (value is XmlAttribute)
                            {
                                strInVal = (value as XmlAttribute).Value;
                            }
                            else
                            {
                                strInVal = value.ToString().ToLower();
                            }
                            //MessageBox.Show(strInVal);
                            //开始执行表达式
                            if (ExpressionHandler.IsMatch(strInVal, exArray[0]))
                            {
                                object outValue = TryGetValueFromResource(exArray[1]);
                                if (outValue == null)
                                {
                                    continue;
                                }
                                return outValue;
                            }
                        }
                    }
                    return outDefaultVal;
                }
                else
                {
                    return value;
                }
            }
            catch
            {
                if (outDefaultVal != null)
                {
                    return outDefaultVal;
                }
                return value;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class ExpressionHandler
    {
        private const string BRACKET = "\\(([^\\(\\)]*?)\\)";

        public static bool IsMatch(string inValue, string filterExpression)
        {
            if (inValue != null)
                inValue = inValue.ToLower();
            filterExpression = filterExpression.ToLower().Trim();
            filterExpression = filterExpression.Replace(" ", "");
            filterExpression = filterExpression.Replace("\r", "");
            filterExpression = filterExpression.Replace("\n", "");
            filterExpression = HandleBracket(filterExpression, inValue);
            return DoMatch(filterExpression, inValue);
        }

        private static string HandleBracket(string filterExpression, string inValue)
        {
            while (Regex.IsMatch(filterExpression, BRACKET))
            {
                Match m = Regex.Match(filterExpression, BRACKET);
                if (m.Success)
                {
                    string toReplace = m.Groups[0].Value;
                    string innerExpression = m.Groups[1].Value;
                    string replacStr = DoMatch(innerExpression, inValue) ? "[TRUE]" : "[FALSE]";
                    filterExpression = filterExpression.Replace(toReplace, replacStr);
                }
            }
            return filterExpression;
        }


        private static bool DoMatch(string filterExpression, string inValue)
        {
            if (string.IsNullOrEmpty(filterExpression))
            {
                return true;
            }
            else if (filterExpression == "![TRUE]" || filterExpression == "！[TRUE]")
            {
                return false;
            }
            else if (filterExpression == "[TRUE]")
            {
                return true;
            }
            else if (filterExpression == "![FALSE]" || filterExpression == "！[FALSE]")
            {
                return true;
            }
            else if (filterExpression == "[FALSE]")
            {
                return false;
            }
            else if (filterExpression.Contains('|'))
            {
                string[] orList = filterExpression.Split('|');
                foreach (string orExpression in orList)
                {
                    if (DoMatch(orExpression, inValue))
                    {
                        return true;
                    }
                }
                return false;
            }
            else if (filterExpression.Contains('&'))
            {
                string[] andList = filterExpression.Split('&');
                foreach (string andExpression in andList)
                {
                    if (!DoMatch(andExpression, inValue))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                bool bRelative = false;
                if (filterExpression.StartsWith("!"))
                {
                    bRelative = true;
                    filterExpression = filterExpression.Substring(1);
                    if (filterExpression == "")
                    {
                        return true;
                    }
                }

                if (bRelative)
                {
                    if (!string.IsNullOrEmpty(inValue) && string.Compare(inValue, filterExpression, true) == 0)
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(inValue))
                    {
                        if (string.Compare(inValue, filterExpression, true) == 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }

            }
        }
    }
}
