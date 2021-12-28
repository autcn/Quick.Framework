
namespace Quick
{
    public static class RegexExpressions
    {
        /// <summary>
        /// 汉字+字母+数字+减号+下划线
        /// </summary>
        public const string ProjectName = @"^[\u2E80-\u9FFFa-zA-Z0-9\-_\s]+$";

        /// <summary>
        /// TaskName
        /// </summary>
        public const string TaskName = @"^[\u2E80-\u9FFFa-zA-Z0-9\-_\s]+$";

        /// <summary>
        /// 本地磁盘路径
        /// </summary>
        public const string LocalPath = @"^[c-zC-Z]:[\s\S]*?$";

        /// <summary>
        /// 文件名中的非法字符
        /// </summary>
        public const string FileNameInvalidChars = @"[\\/:\?\*\|<>" + "\"]";

        /// <summary>
        /// 全汉字
        /// </summary>
        public const string AllChinese = @"^[\u2E80-\u9FFF]+$";

        /// <summary>
        /// 有汉字
        /// </summary>
        public const string PartChinese = @"[\u2E80-\u9FFF]";

        /// <summary>
        /// IP地址
        /// </summary>
        public const string IpAddress = @"^((25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)\.){3}((25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d))$";

        /// <summary>
        /// 时间
        /// </summary>
        public const string Time = @"^(([0-1]\d)|(2[0-3])|(\d)):[0-5]\d$";
    }
}
