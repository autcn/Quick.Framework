using System;
using System.Windows;

namespace Quick
{
    public static class QEditWindowHelper
    {
        private static void SetWindowOptions(Window window, QEditWindowOptions options)
        {
            window.Width = options.DefaultWidth;
            window.Height = options.DefaultHeight;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        public static EditWindowContext<T> Create<T>()
        {
            T tObj = QServiceProvider.GetService<T>();
            QEditWindowOptions options = QServiceProvider.GetService<QEditWindowOptions>();
            Window window = (Window)Activator.CreateInstance(options.WindowType);
            SetWindowOptions(window, options);
            QEditWindowContent wndContent = new QEditWindowContent(window, tObj);
            var editContext = new EditWindowContext<T>(window, wndContent, tObj);
            window.ShowDialog();
            return editContext;
        }

        public static EditWindowContext<T> Create<T>(T viewModel)
        {
            QEditWindowOptions options = QServiceProvider.GetService<QEditWindowOptions>();
            Window window = (Window)Activator.CreateInstance(options.WindowType);
            SetWindowOptions(window, options);
            QEditWindowContent wndContent = new QEditWindowContent(window, viewModel);
            var editContext = new EditWindowContext<T>(window, wndContent, viewModel);
            return editContext;
        }

        public static EditWindowContext<T> ShowEditDialog<T>(Window owner, Action<EditWindowContext<T>> action)
        {
            T tObj = QServiceProvider.GetService<T>();
            QEditWindowOptions options = QServiceProvider.GetService<QEditWindowOptions>();
            Window window = (Window)Activator.CreateInstance(options.WindowType);
            SetWindowOptions(window, options);
            QEditWindowContent wndContent = new QEditWindowContent(window, tObj);
            var editContext = new EditWindowContext<T>(window, wndContent, tObj);
            action?.Invoke(editContext);
            window.Owner = owner ?? Application.Current.MainWindow;
            window.ShowDialog();
            return editContext;
        }

        public static EditWindowContext<T> ShowEditDialog<T>(Action<EditWindowContext<T>> action)
        {
            return ShowEditDialog<T>((Window)null, action);
        }

        public static EditWindowContext<T> ShowEditDialog<T>()
        {
            return ShowEditDialog<T>((Action<EditWindowContext<T>>)null);
        }

        public static EditWindowContext<T> ShowEditDialog<T>(Window owner, T viewModel, Action<EditWindowContext<T>> action)
        {
            QEditWindowOptions options = QServiceProvider.GetService<QEditWindowOptions>();
            Window window = (Window)Activator.CreateInstance(options.WindowType);
            SetWindowOptions(window, options);
            QEditWindowContent wndContent = new QEditWindowContent(window, viewModel);
            var editContext = new EditWindowContext<T>(window, wndContent, viewModel);
            action?.Invoke(editContext);
            window.Owner = owner ?? Application.Current.MainWindow;
            window.ShowDialog();
            return editContext;
        }

        public static EditWindowContext<T> ShowEditDialog<T>(T viewModel, Action<EditWindowContext<T>> action)
        {
            return ShowEditDialog<T>(null, viewModel, action);
        }

        public static EditWindowContext<T> ShowEditDialog<T>(T viewModel)
        {
            return ShowEditDialog<T>(null, viewModel, null);
        }
    }

    public class EditWindowContext<T>
    {
        public EditWindowContext(Window window, QEditWindowContent editContent, T viewModel)
        {
            Window = window;
            Content = editContent;
            ViewModel = viewModel;
        }
        public Window Window { get; }
        public QEditWindowContent Content { get; }
        public T ViewModel { get; }
    }
}
