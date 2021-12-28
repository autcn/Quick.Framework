using Quick;
using System.ComponentModel;

namespace MaterialThemeDemo.ViewModel
{
    [EnumNullDescription("未选择")] //定义不选择时的提示文本
    public enum OccupationType
    {
        [Description("@Demo.Control.Edit.Occupation.Student")] //使用资源，以便多语言支持
        Student,

        [Description("医生")]
        Doctor,

        [Description("教师")]
        Teacher
    }
    public enum GenderType
    {
        Men,
        Women
    }
}
