using DevExpress.Mvvm;
using System;

namespace Quick
{
    public class Messenger : IMessenger
    {
        private static Lazy<Messenger> g_messenger = new Lazy<Messenger>(() => new Messenger());
        public static Messenger Default => g_messenger.Value;

        public void Register<TMessage>(object recipient, object token, bool receiveInheritedMessages, Action<TMessage> action)
        {
            DevExpress.Mvvm.Messenger.Default.Register<TMessage>(recipient, token, receiveInheritedMessages, action);
        }
        public void Send<TMessage>(TMessage message, Type messageTargetType, object token)
        {
            DevExpress.Mvvm.Messenger.Default.Send<TMessage>(message, messageTargetType, token);
        }

        public void Unregister(object recipient)
        {
            DevExpress.Mvvm.Messenger.Default.Unregister(recipient);
        }

        public void Unregister<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            DevExpress.Mvvm.Messenger.Default.Unregister<TMessage>(recipient, token, action);
        }

        //Extensions

        public void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            DevExpress.Mvvm.Messenger.Default.Register<TMessage>(recipient, action);
        
        }
        public void Register<TMessage>(object recipient, bool receiveInheritedMessagesToo, Action<TMessage> action)
        {
            DevExpress.Mvvm.Messenger.Default.Register<TMessage>(recipient, receiveInheritedMessagesToo, action);
        }

        public void Register<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            DevExpress.Mvvm.Messenger.Default.Register<TMessage>(recipient, token, action);
        }

        public void Send<TMessage>(TMessage message)
        {
            DevExpress.Mvvm.Messenger.Default.Send<TMessage>(message);
        }

        public void Send<TMessage, TTarget>(TMessage message)
        {
            DevExpress.Mvvm.Messenger.Default.Send<TMessage, TTarget>(message);
        }

        public void Send<TMessage>(TMessage message, object token)
        {
            DevExpress.Mvvm.Messenger.Default.Send<TMessage>(message, token);
        }
        public void Unregister<TMessage>(object recipient)
        {
            DevExpress.Mvvm.Messenger.Default.Unregister<TMessage>(recipient);
        }

        public void Unregister<TMessage>(object recipient, object token)
        {
            DevExpress.Mvvm.Messenger.Default.Unregister<TMessage>(recipient, token);
        }
        public void Unregister<TMessage>(object recipient, Action<TMessage> action)
        {
            DevExpress.Mvvm.Messenger.Default.Unregister<TMessage>(recipient, action);
        }

    }
}
