namespace Quick
{
    /// <summary>
    /// ComboBox绑定目标
    /// </summary>
    public enum ComboBoxBindType
    {
        Text,
        Value,
        Item
    }

    public class QComboBoxAttribute : QEditAttribute
    {
        public QComboBoxAttribute()
        {
            // Alignment = HorizontalAlignment.Center;
        }

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string ItemsSourcePath { get; set; }

        /// <summary>
        /// ComboBox数据源值成员
        /// </summary>
        public string SelectedValuePath { get; set; }

        /// <summary>
        /// ComboBox显示成员
        /// </summary>
        public string DisplayMemberPath { get; set; }

        /// <summary>
        /// 组合框绑定的类型， 默认绑定SelectedItem
        /// </summary>
        public ComboBoxBindType BindType { get; set; } = ComboBoxBindType.Item;
    }
}
