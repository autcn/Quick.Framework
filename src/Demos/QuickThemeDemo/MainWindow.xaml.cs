using Quick;
using QuickThemeDemo.ViewModel;
using System;
using System.Windows;

namespace QuickThemeDemo
{
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _vm;
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            this.DataContext = _vm;
        }

        private void btnEditStudentSimple_Click(object sender, RoutedEventArgs e)
        {
            QEditWindowHelper.ShowEditDialog(_vm.SimpleStudent);
        }

        private void btnEditStudentAdvance_Click(object sender, RoutedEventArgs e)
        {
            QEditWindowHelper.ShowEditDialog(_vm.AdvancedStudent);
        }

        private void btnEditStudentMutiLang_Click(object sender, RoutedEventArgs e)
        {
            QEditWindowHelper.ShowEditDialog(_vm.LanguageStudent);
        }

        private void btnTextDemo_Click(object sender, RoutedEventArgs e)
        {
            QEditWindowHelper.ShowEditDialog<TextBoxDemoViewModel>();
        }

        private void btnComboBoxDemo_Click(object sender, RoutedEventArgs e)
        {
            QEditWindowHelper.ShowEditDialog<ComboBoxDemoViewModel>();
        }

        private void btnEnumDemo_Click(object sender, RoutedEventArgs e)
        {
            QEditWindowHelper.ShowEditDialog<EnumDemoViewModel>();
        }

        private void btnOtherControlDemo_Click(object sender, RoutedEventArgs e)
        {
            var wnd = QEditWindowHelper.ShowEditDialog<OtherControlsDemoViewModel>();
            var val = wnd.Content.EditVM;
        }

        private void btnQEditPanelDemo_Click(object sender, RoutedEventArgs e)
        {
            EditPanelDemoWindow window = QServiceProvider.GetService<EditPanelDemoWindow>();
            window.Owner = this;
            window.ShowDialog();
        }

        private void btnDataGridDemo_Click(object sender, RoutedEventArgs e)
        {
            QEditWindowHelper.ShowEditDialog<DataGridDemoViewModel>(context => 
            {
                context.Window.Width = 800;
            });
        }
    }
}
