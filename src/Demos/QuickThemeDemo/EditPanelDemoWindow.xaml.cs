using Quick;
using QuickThemeDemo.ViewModel;

namespace QuickThemeDemo
{
    /// <summary>
    /// EditPanelDemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditPanelDemoWindow : QWindow
    {
        private EditPanelDemoViewModel _vm;
        public EditPanelDemoWindow(EditPanelDemoViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            this.DataContext = _vm;
        }
    }
}
