namespace Quick
{
    public class QSliderAttribute : QEditAttribute
    {
        /// <summary>
        /// 滑动控件的最小值
        /// </summary>
        public double Min { get; set; } = 0;

        /// <summary>
        /// 滑动控件的最大值
        /// </summary>
        public double Max { get; set; } = 100;

        /// <summary>
        ///  数值的样式
        /// </summary>
        public string ValueLabelStyleKey { get; set; }
    }
}
