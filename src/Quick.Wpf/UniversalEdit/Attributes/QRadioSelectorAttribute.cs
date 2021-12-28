using System.Windows.Controls;

namespace Quick
{
    /// <summary>
    /// RadioSelector绑定目标
    /// </summary>
    public enum RadioSelectorBindType
    {
        Value,
        Item
    }

    public class QRadioSelectorAttribute : QEditAttribute
    {
        public QRadioSelectorAttribute()
        {
        }

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string ItemsSourcePath { get; set; }

        /// <summary>
        /// 数据源值成员
        /// </summary>
        public string SelectedValuePath { get; set; }

        /// <summary>
        /// 显示成员
        /// </summary>
        public string ContentMemberPath { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public Orientation Orientation { get; set; } = Orientation.Horizontal;

        /// <summary>
        /// 绑定的类型， 默认绑定SelectedItem
        /// </summary>
        public RadioSelectorBindType BindType { get; set; } = RadioSelectorBindType.Item;
    }
}
