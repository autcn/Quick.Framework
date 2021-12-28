using System.Windows;

namespace Quick
{
    public interface IMessageBox
    {
        MessageBoxResult Show(Window owner, string text, string title, MessageBoxButton button, MessageBoxImage icon);
    }
}
