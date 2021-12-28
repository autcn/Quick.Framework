using Autofac;
using System.Windows;

namespace Quick
{
    //public enum MessageBoxType
    //{
    //    Quick,
    //    System
    //}
    public static class QMessageBoxExtensions
    {
        public static void AddMessageBox(this ContainerBuilder serviceBuilder)
        {
            serviceBuilder.RegisterType<SystemMessageBox>().As<IMessageBox>().SingleInstance();
            //serviceBuilder.RegisterType<QMessageBox>().As<IMessageBox>().SingleInstance();
        }

        private static string GetDefaultTitle()
        {
            return QLocalizationProperties.StMsgBoxDefaultWndTitle;
        }

        public static void Show(this IMessageBox messageBox, string text)
        {
            messageBox.Show(text, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void Show(this IMessageBox messageBox, string text, string title)
        {
            messageBox.Show(text, GetDefaultTitle(), MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void Show(this IMessageBox messageBox, string text, MessageBoxImage icon)
        {
            messageBox.Show(text, MessageBoxButton.OK, icon);
        }
        public static void Show(this IMessageBox messageBox, string text, string title, MessageBoxImage icon)
        {
            messageBox.Show(text, title, MessageBoxButton.OK, icon);
        }

        public static void Show(this IMessageBox messageBox, Window owner, string text)
        {
            messageBox.Show(owner, text, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void Show(this IMessageBox messageBox, Window owner, string text, string title)
        {
            messageBox.Show(owner, text, GetDefaultTitle(), MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void Show(this IMessageBox messageBox, Window owner, string text, MessageBoxImage icon)
        {
            messageBox.Show(owner, text, MessageBoxButton.OK, icon);
        }
        public static void Show(this IMessageBox messageBox, Window owner, string text, string title, MessageBoxImage icon)
        {
            messageBox.Show(owner, text, title, MessageBoxButton.OK, icon);
        }

        public static MessageBoxResult Show(this IMessageBox messageBox, string text, MessageBoxButton button, MessageBoxImage icon)
        {
            return messageBox.Show(text, GetDefaultTitle(), button, icon);
        }

        public static MessageBoxResult Show(this IMessageBox messageBox, string text, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            return messageBox.Show(null, text, title, button, icon);
        }

        public static MessageBoxResult Show(this IMessageBox messageBox, Window owner, string text, MessageBoxButton button, MessageBoxImage icon)
        {
            return messageBox.Show(owner, text, GetDefaultTitle(), button, icon);
        }

        public static MessageBoxResult QuestionOKCancel(this IMessageBox messageBox, string text)
        {
            return messageBox.Show(text, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        public static MessageBoxResult QuestionYesNo(this IMessageBox messageBox, string text)
        {
            return messageBox.Show(text, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static MessageBoxResult QuestionOKCancel(this IMessageBox messageBox, string text, MessageBoxImage image)
        {
            return messageBox.Show(text, MessageBoxButton.OKCancel, image);
        }

        public static MessageBoxResult QuestionYesNo(this IMessageBox messageBox, string text, MessageBoxImage image)
        {
            return messageBox.Show(text, MessageBoxButton.YesNo, image);
        }

        public static MessageBoxResult QuestionOKCancel(this IMessageBox messageBox, Window owner, string text)
        {
            return messageBox.Show(owner, text, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        public static MessageBoxResult QuestionYesNo(this IMessageBox messageBox, Window owner, string text)
        {
            return messageBox.Show(owner, text, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static MessageBoxResult QuestionOKCancel(this IMessageBox messageBox, Window owner, string text, MessageBoxImage image)
        {
            return messageBox.Show(owner, text, MessageBoxButton.OKCancel, image);
        }

        public static MessageBoxResult QuestionYesNo(this IMessageBox messageBox, Window owner, string text, MessageBoxImage image)
        {
            return messageBox.Show(owner, text, MessageBoxButton.YesNo, image);
        }
    }
}
