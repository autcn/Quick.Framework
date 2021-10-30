using Quick;
using Serilog;
using System.ComponentModel;

namespace MultiLanguageDemo
{
    [SingletonDependency]
    public class MainWindowViewModel : QBindableAppBase, IInitializable
    {
        public void Initialize()
        {
            //注册语言切换事件，用代码处理语言变化
            Localization.RegisterCultureChangedHandler(this, l =>
            {
                string info = l.Format("App.CurrentLanguageChangedLog", l.CurrentCulture.Name);
                Logger.Information(info);
            });
        }

        //使用标记，UI自动生成控件和绑定，在其他Demo中会专门介绍
        [QEnumComboBox]
        public OccupationType? SelectedOccupation { get; set; }

        public void SetLanguage(string cultureName)
        {
            Localization.SetCulture(cultureName);
        }

        public void GetCurrentLanguage()
        {
            MsgBox.Show(Localization.CurrentCulture.Name);
        }

        public void GetMyOccupation()
        {
            if (SelectedOccupation == null)
            {
                //有些函数支持直接字符串或搜索资源时，可以在前面加上@表示搜索资源字符串，如果不加@则显示直接文本
                MsgBox.Show("@Demo.Edit.NotSelectOccupationMsgTip");
            }
            else
            {
                string enumDesc = EnumHelper.GetEnumValueDesc<OccupationType>(SelectedOccupation);
                string info = Localization.Format("Demo.Edit.HasSelectedOccupationMsgTip", enumDesc);
                MsgBox.Show(info);
            }
        }


    }

    [EnumNullDescription("@Demo.Edit.Occupation.NoSelect")] //定义不选择时的提示文本
    public enum OccupationType
    {
        [Description("@Demo.Edit.Occupation.Student")] //使用资源，以便多语言支持
        Student,


        [Description("医生")]  //不使用资源，不会根据语言变化
        Doctor,

        [Description("@Demo.Edit.Occupation.Teacher")] //使用资源，以便多语言支持
        Teacher
    }
}
