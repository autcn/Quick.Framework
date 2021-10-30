using Quick;
using System.ComponentModel.DataAnnotations;

namespace AutoUIDemo.ViewModel
{
    public class TextBoxDemoViewModel : QEditBindableBase
    {
        /// <summary>
        /// 无限制
        /// </summary>
        [QTextBox]
        public string Name { get; set; }

        /// <summary>
        /// 必填
        /// </summary>
        [QTextBox]
        [QRequired]
        public string Name2 { get; set; }

        /// <summary>
        /// 长度约束，必须3-6个字符
        /// </summary>
        [QTextBox]
        [QRequired]
        [StringLength(6, MinimumLength = 3)]
        public string Name3 { get; set; }

        /// <summary>
        /// 自定义Title，正则限制小写字母
        /// </summary>
        [QTextBox(Title = "名字", WaterMark = "请输入名字")]
        [QRequired]
        [QRegularExpression("^[a-z]+$")]  //使用内置的QRegularExpression以便支持多语言提示
        public string Name4 { get; set; }

        /// <summary>
        /// 限制只能输入数字，从资源中指定Title，以便支持多语言
        /// </summary>
        [QTextBox(Title = "@Demo.Control.Edit.PhoneTitle", InputChars = InputChars.Number)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 限制只能输入数字，允许负数
        /// </summary>
        [QTextBox(InputChars = InputChars.Number | InputChars.Negative)]
        public string Name7 { get; set; }

        /// <summary>
        /// 限制输入IP字符
        /// </summary>
        [QTextBox(InputChars = InputCharsModels.IpAddress)]
        [QRegularExpression(RegexExpressions.IpAddress, ErrorMessage = "IP地址非法")]
        public string IpAddress { get; set; }

        /// <summary>
        /// 限制输入时间字符
        /// </summary>
        [QTextBox(InputChars = InputCharsModels.Time)]
        [QRegularExpression(RegexExpressions.Time)]
        public string Time { get; set; }

        [QTextBox]
        public int Age { get; set; }


        [QTextBox]
        [Required(ErrorMessage = "年龄不能为空")] //自定义错误提示
        public int? Age2 { get; set; }

        /// <summary>
        /// Remark示例
        /// </summary>
        [QTextBox(Width = "80", Remark = "岁")]
        [Range(12, 60, ErrorMessage = "@Demo.Control.Edit.AgeRangeTip")] //自定义错误提示，并使用字符串资源，以便支持多语言提示
        public int? Age3 { get; set; }

        /// <summary>
        /// 浮点示例
        /// </summary>
        [QTextBox]
        public double Height { get; set; }

        /// <summary>
        /// 限制两位小数
        /// </summary>
        [QTextBox(Width = "80", StringFormat = "F2", Title = "身高", Remark = "@Demo.Control.Edit.HeightUnitRemark")]
        public double Height2 { get; set; }

        /// <summary>
        /// 允许为空
        /// </summary>
        [QTextBox]
        public double? Height3 { get; set; }

    }
}
