using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Quick
{
    public class ChineseToPinyin : IChineseToPinyin, IInitializable
    {
        private Dictionary<string, string> _pinYinMap;

        ///   <summary> 
        ///   汉字转拼音 
        ///   </summary> 
        ///   <param   name="txt"> 需要转换的汉字 </param> 
        ///   <returns> 返回汉字对应的拼音 </returns> 
        public string ToPinYin(string txt)
        {
            txt = txt.Trim();
            byte[] arr = new byte[2];   //每个汉字为2字节 
            StringBuilder result = new StringBuilder();//使用StringBuilder优化字符串连接
            char[] arrChar = txt.ToCharArray();
            for (int j = 0; j < arrChar.Length; j++)   //遍历输入的字符 
            {
                arr = System.Text.Encoding.Default.GetBytes(arrChar[j].ToString());//根据系统默认编码得到字节码 
                if (arr.Length == 1)//如果只有1字节说明该字符不是汉字，结束本次循环 
                {
                    result.Append(arrChar[j].ToString());
                    continue;

                }
                string zi = arrChar[j].ToString();
                string orgPinYin = null;
                if (_pinYinMap.TryGetValue(zi, out orgPinYin))
                {
                    if (orgPinYin.Contains(","))
                    {
                        string[] multiPinYin = orgPinYin.Split(',');
                        orgPinYin = multiPinYin[0];
                    }
                    result.Append(" " + orgPinYin + " ");
                }
                else
                {
                    result.Append(arrChar[j].ToString());
                }
            }
            string strRes = result.ToString().Trim().ToLower();
            while (strRes.IndexOf("  ") != -1)
            {
                strRes = strRes.Replace("  ", " ");
            }
            return strRes;
        }


        public string ToPinYinMulti(string txt)
        {
            txt = txt.Trim();
            byte[] arr = new byte[2];   //每个汉字为2字节 
            StringBuilder result = new StringBuilder();//使用StringBuilder优化字符串连接
            char[] arrChar = txt.ToCharArray();
            for (int j = 0; j < arrChar.Length; j++)   //遍历输入的字符 
            {
                arr = System.Text.Encoding.Default.GetBytes(arrChar[j].ToString());//根据系统默认编码得到字节码 
                if (arr.Length == 1)//如果只有1字节说明该字符不是汉字，结束本次循环 
                {
                    result.Append(arrChar[j].ToString());
                    continue;

                }
                string zi = arrChar[j].ToString();
                string orgPinYin = null;
                if (_pinYinMap.TryGetValue(zi, out orgPinYin))
                {
                    result.Append(" " + orgPinYin + " ");
                }
                else
                {
                    result.Append(arrChar[j].ToString());
                }
            }
            string strRes = result.ToString().Trim().ToLower();
            while (strRes.IndexOf("  ") != -1)
            {
                strRes = strRes.Replace("  ", " ");
            }
            return strRes;
        }

        private List<StringBuilder> ExtendAndAppend(List<StringBuilder> builders, string[] fields)
        {
            if (builders == null || builders.Count == 0 || fields == null || fields.Length == 0)
            {
                return builders;
            }
            //先扩展和append
            int copyEndIndex = builders.Count - 1;
            for (int i = 1; i < fields.Length; i++)
            {
                for (int j = 0; j <= copyEndIndex; j++)
                {
                    StringBuilder tempBuilder = new StringBuilder(builders[j].ToString());
                    tempBuilder.Append(" " + fields[i] + " ");
                    builders.Add(tempBuilder);
                }
            }
            //补上最前面的
            for (int i = 0; i <= copyEndIndex; i++)
            {
                builders[i].Append(" " + fields[0] + " ");
            }
            return builders;
        }

        /// <summary>
        /// 最多两个拼音
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public List<string> ToPinYinArray(string txt)
        {
            txt = txt.Trim();
            byte[] arr = new byte[2];   //每个汉字为2字节 
            List<StringBuilder> results = new List<StringBuilder>();//使用StringBuilder优化字符串连接
            results.Add(new StringBuilder());
            char[] arrChar = txt.ToCharArray();
            for (int j = 0; j < arrChar.Length; j++)   //遍历输入的字符 
            {
                arr = System.Text.Encoding.Default.GetBytes(arrChar[j].ToString());//根据系统默认编码得到字节码 
                if (arr.Length == 1)//如果只有1字节说明该字符不是汉字，结束本次循环 
                {
                    foreach (StringBuilder result in results)
                    {
                        result.Append(arrChar[j].ToString());
                    }
                    continue;

                }
                string zi = arrChar[j].ToString();
                string orgPinYin = null;
                if (_pinYinMap.TryGetValue(zi, out orgPinYin))
                {

                    string[] multiPinYin = orgPinYin.Split(',');
                    ExtendAndAppend(results, multiPinYin);
                }
                else
                {
                    foreach (StringBuilder result in results)
                    {
                        result.Append(arrChar[j].ToString());
                    }
                }


            }
            List<string> returnValues = new List<string>();
            foreach (StringBuilder result in results)
            {
                string strRes = result.ToString().Trim().ToLower();
                while (strRes.IndexOf("  ") != -1)
                {
                    strRes = strRes.Replace("  ", " ");
                }
                returnValues.Add(strRes);
            }
            return returnValues;
        }

        public string PinYinToFirstChar(string strOrgPinYin, bool zcsDouble)
        {
            if (string.IsNullOrWhiteSpace(strOrgPinYin))
            {
                return "";
            }
            string[] szWord = strOrgPinYin.Split(' ');
            string res = "";
            foreach (string word in szWord)
            {
                if (zcsDouble && (word.StartsWith("zh") || word.StartsWith("ch") || word.StartsWith("sh")))
                {
                    res += word.Substring(0, 2);
                }
                else
                {
                    res += word.Substring(0, 1);
                }
            }
            return res;
        }

        public bool IsQuickSearchMatch(string keyword, string toCompoareText)
        {
            //先拿到输入的值
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return false;
            }
            string inputLower = keyword.ToLower();
            //拿到当前条目的字符串值
            string strItem = toCompoareText;
            if (string.IsNullOrWhiteSpace(strItem))
            {
                return false;
            }
            strItem = strItem.ToLower();
            if (Regex.IsMatch(strItem, RegexExpressions.PartChinese))
            {
                //对于汉字先验证汉语拼音全拼和首字母拼音
                List<string> orgPinYinList = ToPinYinArray(strItem);
                foreach (string orgPinYin in orgPinYinList)
                {
                    string noSpacePinYin = orgPinYin.Replace(" ", "");
                    string firstCharPinYin = PinYinToFirstChar(orgPinYin, false);
                    string firstCharPinYinDouble = PinYinToFirstChar(orgPinYin, true);
                    string noSpaceInput = inputLower;
                    while (noSpaceInput.IndexOf(" ") != -1)
                    {
                        noSpaceInput = noSpaceInput.Replace(" ", "");
                    }
                    if (noSpacePinYin.Contains(noSpaceInput) || firstCharPinYin.Contains(noSpaceInput)
                        || firstCharPinYinDouble.Contains(noSpaceInput))
                    {
                        return true;
                    }
                }
            }
            if (strItem.Contains(inputLower))
            {
                return true;
            }
            return false;
        }

        public void Initialize()
        {
            if (_pinYinMap == null)
            {
                _pinYinMap = new Dictionary<string, string>();
            }
            else
            {
                return;
            }
            string mapText = Assembly.GetExecutingAssembly().GetManifestResourceString("Quick.Text.PinYinMap.txt", Encoding.UTF8);
            string[] segments = mapText.Split('_');
            foreach (string seg in segments)
            {
                string[] fields = seg.Split('|');
                _pinYinMap.Add(fields[0], fields[1]);
            }
        }
    }
}
