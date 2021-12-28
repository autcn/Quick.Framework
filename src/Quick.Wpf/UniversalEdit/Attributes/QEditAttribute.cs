using System;
using System.Windows;
using System.Windows.Data;

namespace Quick
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QEditAttribute : Attribute
    {
        /// <summary>
        /// 标题(当不设置标题时，默认取属性名称)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// 分组标题，必须设置值代表分组
        /// </summary>
        public string GroupHeader { get; set; }

        /// <summary>
        /// 组的顺序
        /// </summary>
        public int? GroupOrder { get; set; }

        /// <summary>
        /// 后置注释
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///  标题自定义样式的资源Key，搜索顺序为控件自身的资源，全局资源
        /// </summary>
        public string TitleStyleKey { get; set; }

        /// <summary>
        ///  数据输入控件样式的资源Key
        /// </summary>
        public string InputStyleKey { get; set; }

        /// <summary>
        ///  注释样式的资源Key，搜索顺序为控件自身的资源，全局资源
        /// </summary>
        public string RemarkStyleKey { get; set; }

        /// <summary>
        /// 数据输入控件自定义宽度，格式为"100"或"0.4*"，不带星号单位为像素，带星号为比例，必须为0-1之间
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// 数据绑定的方向设置
        /// </summary>
        public BindingMode BindingMode { get; set; } = BindingMode.Default;

        /// <summary>
        /// 绑定的格式
        /// </summary>
        public string StringFormat { get; set; }

        /// <summary>
        /// 只读
        /// </summary>
        public bool IsReadOnly { get; set; } = false;

        /// <summary>
        /// 可操作
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 控件显示放置位置
        /// </summary>
        public QEditPlaces Place { get; set; } = QEditPlaces.All;

        /// <summary>
        /// DataGrid列内容对齐方式
        /// </summary>
        public TextAlignment DataGridColumnAlignment { get; set; } = TextAlignment.Center;

        /// <summary>
        /// 可编辑控件内容对齐方式
        /// </summary>
        public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Left;

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string Tag { get; set; }

        public void Import(QEditAttribute attr)
        {
            this.Title = attr.Title;
            this.Order = attr.Order;
            this.GroupHeader = attr.GroupHeader;
            this.GroupOrder = attr.GroupOrder;
            this.Remark = attr.Remark;
            this.TitleStyleKey = attr.TitleStyleKey;
            this.InputStyleKey = attr.InputStyleKey;
            this.RemarkStyleKey = attr.RemarkStyleKey;
            this.Width = attr.Width;
            this.BindingMode = attr.BindingMode;
            this.StringFormat = attr.StringFormat;
            this.IsReadOnly = attr.IsReadOnly;
            this.IsEnabled = attr.IsEnabled;
            this.Place = attr.Place;
            this.DataGridColumnAlignment = attr.DataGridColumnAlignment;
            this.Alignment = attr.Alignment;
            this.Tag = attr.Tag;
        }
    }

    public enum QEditPlaces
    {
        All,
        DataGrid,
        EditControl
    }
}
