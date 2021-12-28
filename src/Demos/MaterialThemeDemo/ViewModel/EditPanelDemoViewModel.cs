using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialThemeDemo.Creators;
using Quick;

namespace MaterialThemeDemo.ViewModel
{
    public class EditPanelDemoViewModel : QValidatableBase
    {
        [QTextBox]
        public string Name { get; set; }

        //可以指定输入控件的Style Key
        [QTextBox(InputStyleKey = "@TestTextBoxStyle")]
        public string Phone { get; set; }

        [QEnumComboBox]
        public OccupationType Occupation { get; set; }

        public string Info { get; set; } = "自动UI与常规方法混合";

        //自定义的自动UI特性
        [MyColorPicker]
        public string FavoriteColor { get; set; } = "#FFFFFFFF";

    }
}
