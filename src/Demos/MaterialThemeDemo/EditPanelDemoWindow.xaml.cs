using Quick;
using MaterialThemeDemo.ViewModel;

namespace MaterialThemeDemo
{
    /// <summary>
    /// EditPanelDemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditPanelDemoWindow
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
