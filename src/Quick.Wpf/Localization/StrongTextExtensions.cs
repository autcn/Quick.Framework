namespace Quick
{
    public static class StrongTextExtensions
    {
        //public static bool IsStrongText(this string str)
        //{
        //    if (str == null)
        //    {
        //        return false;
        //    }
        //    return str.StartsWith(QLocalizationProperties.ResourceKeyPrefix);
        //}

        public static bool IsResourceKey(this string str)
        {
            if (str == null)
            {
                return false;
            }
            return str.StartsWith(QLocalizationProperties.ResourceKeyPrefix) && !str.StartsWith(QLocalizationProperties.ResourceKeyPrefixEscape);
        }

        public static string StrongTextToNormal(this string strongText)
        {
            if (strongText == null)
            {
                return strongText;
            }
            if (strongText.IsResourceKey())
            {
                return strongText.Substring(1);
            }
            return strongText;
        }
    }
}
