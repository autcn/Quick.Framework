using Quick;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AutoUIDemo.ViewModel
{
    public class OtherControlsDemoViewModel : QEditBindableBase
    {

        [QTime(Title = "上学时间", GroupHeader = "asdf")]
        [QRequired]
        public DateTime? Time2 { get; set; }

        [QTextBox]
        public string Name { get; set; }

        [QLabel]
        public string Race { get; set; } = "汉族";

        [QLabel]
        public string Province { get; set; } = "北京市";

        [QTextBox]
        public int? Age { get; set; }

        [QTextBox(Title = "体重", Remark = "KG", StringFormat = "F01", Width = "80")]
        [QRequired]
        public double? Weight { get; set; }

        [QCheckBox]
        public bool IsChild { get; set; }

        [QToggleButton(InputStyleKey = "@AutoUIToggleBtnStyle")]
        public bool IsForeigner { get; set; }

        [QDatePicker(Title = "生日")]
        [QRequired]
        public DateTime? DateOfBirth { get; set; }

        [QDatePicker(Title = "生日2")]
        [QRequired]
        public string DateOfBirth2 { get; set; } = "2021/09/12";

        [QPasswordBox(Title = "密码")]
        public string Password { get; set; }

        [QSlider(Title = "完成度", Min = 0, Max = 100 )]
        public int Percent { get; set; } = 40;

        [QTime(Title = "上学时间")]
        [QRequired]
        public string Time { get; set; } = "17:55";

        [QSpinner(MinNumber = -100)]
        public int Level { get; set; }

        [QFilePicker(Title = "照片", IsReadOnly = false, Filter = "picture files|*.jpg;*.png")]
        [QRequired]
        public string ImagePath { get; set; }

        /// <summary>
        /// 只读
        /// </summary>
        [QFilePicker(Title = "照片2", IsReadOnly = true, Filter = "picture files|*.jpg;*.png")]
        [QRequired]
        public string ImagePath2 { get; set; }

        /// <summary>
        /// 文件夹
        /// </summary>
        [QFilePicker(Title = "文件夹", IsFolderPicker = true)]
        [QRequired]
        public string Folder { get; set; }

        [QIPAddress]
        [QRequired]
        public IPAddress Address { get; set; }

        [QIPAddress]
        public string Address2 { get; set; } = "192.168.200.123";

    }

    public class StatusItem : QBindableBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
