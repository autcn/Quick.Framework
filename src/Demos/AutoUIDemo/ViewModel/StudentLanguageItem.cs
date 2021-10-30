using Quick;
using System;
using System.ComponentModel.DataAnnotations;

namespace AutoUIDemo.ViewModel
{
    public class StudentLanguageItem : QValidatableBase
    {
        [QTextBox(GroupHeader = "@Student.Edit.Group.Base", Title = "@Student.Edit.Title.Name")]
        [QRequired] //使用内置的QRequired以便支持多语言提示
        public string Name { get; set; }

        [QEnumComboBox(Title = "@Student.Edit.Title.Gender")]
        [QRequired]
        public GenderType GenderCode { get; set; }

        [QTextBox(Title = "@Student.Edit.Title.Phone")]
        [QRequired]
        [QRegularExpression("^(13[0-9]|14[5-9]|15[0-3,5-9]|16[2,5,6,7]|17[0-8]|18[0-9]|19[0-3,5-9])\\d{8}$")]  //使用内置的QRegularExpression以便支持多语言提示
        public string PhoneNumber { get; set; }

        [QTextBox(Title = "@Student.Edit.Title.Age")]
        [Range(1, 99)]
        public int Age { get; set; }

        [QDatePicker(Title = "@Student.Edit.Title.DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [QTextBox(GroupHeader = "@Student.Edit.Group.Extent", Title = "@Student.Edit.Title.Height", Remark = "@Student.Edit.Remark.Height", Width = "100")]
        public double Height { get; set; }

        [QTextBox(Title = "@Student.Edit.Title.Weight", Remark = "@Student.Edit.Remark.Weight", Width = "100")]
        public double Weight { get; set; }

        [QTextBox(Title = "@Student.Edit.Title.Address")]
        public string Address { get; set; }
    }
}
