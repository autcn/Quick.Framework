using AutoUIDemo.ViewModel;
using Quick;
using System;
using System.Windows;

namespace AutoUIDemo
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
            QEditWindow.ShowEditDialog(_vm.SimpleStudent);
        }

        private void btnEditStudentAdvance_Click(object sender, RoutedEventArgs e)
        {
            QEditWindow.ShowEditDialog(_vm.AdvancedStudent);
        }

        private void btnEditStudentMutiLang_Click(object sender, RoutedEventArgs e)
        {
            QEditWindow.ShowEditDialog(_vm.LanguageStudent);
        }

        private void btnTextDemo_Click(object sender, RoutedEventArgs e)
        {
            QEditWindow.ShowEditDialog<TextBoxDemoViewModel>();
        }

        private void btnComboBoxDemo_Click(object sender, RoutedEventArgs e)
        {
            QEditWindow.ShowEditDialog<ComboBoxDemoViewModel>();
        }

        private void btnEnumDemo_Click(object sender, RoutedEventArgs e)
        {
            QEditWindow.ShowEditDialog<EnumDemoViewModel>();
        }

        private void btnOtherControlDemo_Click(object sender, RoutedEventArgs e)
        {
            QEditWindow.ShowEditDialog<OtherControlsDemoViewModel>();
        }

        private void btnQEditPanelDemo_Click(object sender, RoutedEventArgs e)
        {
            EditPanelDemoWindow window = QServiceProvider.GetService<EditPanelDemoWindow>();
            window.Owner = this;
            window.ShowDialog();
        }

        private void btnDataGridDemo_Click(object sender, RoutedEventArgs e)
        {
            QEditWindow.ShowEditDialog<DataGridDemoViewModel>(context => 
            {
                context.Window.Width = 800;
            });
        }
    }
}
