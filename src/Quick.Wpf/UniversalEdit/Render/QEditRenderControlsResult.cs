using System.Windows;
using System.Windows.Controls;

namespace Quick
{
    public class QEditRenderControlsResult
    {
        //Title
        public FrameworkElement TitleLayoutElement { get; set; }
        public TextBlock RequiredMarkTbk { get; set; }
        public UserControl TitleTbk { get; set; }
        //Body
        public FrameworkElement BodyLayoutElement { get; set; }
        public FrameworkElement InputElement { get; set; }
        public UserControl RemarkTbk { get; set; }

    }
}
