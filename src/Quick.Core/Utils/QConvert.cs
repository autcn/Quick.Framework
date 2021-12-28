using Newtonsoft.Json;
using System;
using System.Text;

namespace Quick
{
    public static class QConvert
    {
        public static string ToBase64String(object obj)
        {
            string strJson = JsonConvert.SerializeObject(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(strJson);
            return Convert.ToBase64String(byteArray);
        }

        public static T FromBase64String<T>(string base64String)
        {
            byte[] byteArray = Convert.FromBase64String(base64String);
            string strJson = Encoding.UTF8.GetString(byteArray);
            return JsonConvert.DeserializeObject<T>(strJson);
        }

    }
}
