using Quick;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuickThemeDemo.ViewModel
{
    public class StudentAdvancedItem : QValidatableBase
    {
        [QTextBox(GroupHeader = "基础信息", Title = "姓名")]
        [QRequired] //使用内置的QRequired以便支持多语言提示
        public string Name { get; set; }

        [QEnumComboBox(Title = "性别")]
        [QRequired]
        public GenderType GenderCode { get; set; }

        [QTextBox(Title = "电话号码")]
        [QRequired]
        [QRegularExpression("^(13[0-9]|14[5-9]|15[0-3,5-9]|16[2,5,6,7]|17[0-8]|18[0-9]|19[0-3,5-9])\\d{8}$")]  //使用内置的QRegularExpression以便支持多语言提示
        public string PhoneNumber { get; set; }

        [QTextBox(Title = "年龄")]
        [Range(1, 99)]
        public int Age { get; set; }

        [QDatePicker(Title = "生日")]
        [QRequired]
        public DateTime? DateOfBirth { get; set; }

        [QTextBox(GroupHeader = "扩展信息", Title = "身高", Remark = "cm", Width = "100")]
        public double Height { get; set; }

        [QTextBox(Title = "体重", Remark = "KG", Width = "100")]
        public double Weight { get; set; }

        [QTextBox(Title = "地址")]
        public string Address { get; set; }
    }

}
