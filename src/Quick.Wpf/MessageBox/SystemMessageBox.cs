using System.Windows;

namespace Quick
{
    internal class SystemMessageBox : IMessageBox
    {
        private ILocalization _localization;
        public SystemMessageBox(ILocalization localization)
        {
            _localization = localization;
        }

        public MessageBoxResult Show(Window owner, string text, string title, MessageBoxButton msgBtn, MessageBoxImage msgIcon)
        {
            title = title ?? "";
            text = text ?? "";
            title = _localization.ConvertStrongText(title);
            text = _localization.ConvertStrongText(text);
            if (owner != null && owner.IsLoaded)
            {
                return MessageBox.Show(owner, text, title, msgBtn, msgIcon);
            }
            return MessageBox.Show(text, title, msgBtn, msgIcon);
        }
    }
}
