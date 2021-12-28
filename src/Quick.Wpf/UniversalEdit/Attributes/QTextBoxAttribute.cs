namespace Quick
{

    public class QTextBoxAttribute : QEditAttribute
    {
        /// <summary>
        /// 输入框输入限制的选项
        /// </summary>
        public InputChars InputChars { get; set; } = InputChars.All;

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength { get; set; } = 0;

        /// <summary>
        /// 水印文字
        /// </summary>
        public string WaterMark { get; set; }
    }
}
