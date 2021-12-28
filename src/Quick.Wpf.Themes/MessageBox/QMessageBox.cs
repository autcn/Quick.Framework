using System;
using System.Windows;

namespace Quick
{
    public class QMessageBox : IMessageBox
    {
        private ILocalization _localization;
        public QMessageBox(ILocalization localization)
        {
            _localization = localization;
        }

        public MessageBoxResult Show(Window owner, string text, string title, MessageBoxButton msgBtn, MessageBoxImage msgIcon)
        {
            QMessageBoxWnd msgBox = new QMessageBoxWnd(_localization, _localization.ConvertStrongText(text), _localization.ConvertStrongText(title), msgBtn, msgIcon);
            if (owner == null)
            {
                if (msgBox != Application.Current.MainWindow
                && Application.Current.MainWindow.IsLoaded)
                {
                    msgBox.Owner = Application.Current.MainWindow;
                }
            }
            else
            {
                msgBox.Owner = owner;
            }
            msgBox.ShowDialog();
            return msgBox.DialogResult;
        }
    }
}
