using AutoMapper;
using EntityFrameworkDemo.Model;
using Quick;
using System;

namespace EntityFrameworkDemo.ViewModel
{
    [QEditObjectDescription("@DatabaseDemo.Edit.Item.StudentInfo")]
    [AutoMap(typeof(Student), ConstructUsingServiceLocator = true, ReverseMap = true)]
    public class StudentItem : DemoEditBindableBase
    {
        public StudentItem()
        {
            Province = "zz";
        }


        [QTextBlock]
        public long Id { get; set; }

        [QTextBox]
        public string Name { get; set; }

        [QTextBox(DataGridColumnAlignment = System.Windows.TextAlignment.Right)]
        public string Race { get; set; } = "汉族";

        [QTextBox]
        public string Province { get; set; } = "北京市";

        [QTextBox]
        public int? Age { get; set; }

        [QTextBox(Title = "体重", Remark = "KG", StringFormat = "F01", Width = "80")]
        [QRequired]
        public double? Weight { get; set; }

        [QToggleButton]
        public bool IsForeigner { get; set; }

        [QDatePicker(Title = "生日")]
        [QRequired]
        public DateTime? DateOfBirth { get; set; }

        [QFilePicker(Title = "照片", IsReadOnly = false, Filter = "picture files|*.jpg;*.png")]
        [QRequired]
        public string ImagePath { get; set; }

    }
}
