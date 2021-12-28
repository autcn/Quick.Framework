using AutoMapper;
using Serilog;
using System;
using System.Runtime.CompilerServices;

namespace Quick
{
    public abstract class QBindableAppBase : QBindableBase
    {
        #region Services

        private IServiceProvider _serviceProvider;
        protected IServiceProvider ServiceProvider => QServiceProvider.LazyGetRequiredService(ref _serviceProvider);

        private ILocalization _localization;
        protected ILocalization Localization => ServiceProvider.LazyGetRequiredService(ref _localization);

        private IMessageBox _messageBox;
        protected IMessageBox MsgBox => ServiceProvider.LazyGetRequiredService(ref _messageBox);

        private IMapper _mapper;
        protected IMapper Mapper => ServiceProvider.LazyGetRequiredService(ref _mapper);

        private ILogger _logger;
        protected ILogger Logger => ServiceProvider.LazyGetRequiredService(ref _logger);

        private ILocalStorage _localStorage;
        protected ILocalStorage Ls => ServiceProvider.LazyGetRequiredService(ref _localStorage);

        private IQConfiguration _config;
        protected IQConfiguration Configuration => ServiceProvider.LazyGetRequiredService(ref _config);

        private IMessenger _messenger;
        protected IMessenger Messenger => ServiceProvider.LazyGetRequiredService(ref _messenger);

        private IFormat _format;
        public IFormat Format => ServiceProvider.LazyGetRequiredService(ref _format);

        #endregion

        #region For DXBinding

        /// <summary>
        /// 该函数用于DXBinding时的属性变化触发器
        /// </summary>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public QBindableAppBase Triggers(params object[] propertyNames)
        {
            return this;
        }

        #endregion

        #region LocalStorage functions

        protected virtual void SetLs<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName.IsNullOrEmpty())
            {
                throw new QException("The propertyName is required.");
            }
            string key = $"{GetType().FullName}.{propertyName}";
            if (Ls.TryGetValue(key, out TValue val))
            {
                if (object.Equals(value, val))
                {
                    return;
                }
            }
            Ls[key] = value;
            NotifyPropertyChanged(propertyName);
        }

        protected virtual TValue GetLs<TValue>([CallerMemberName] string propertyName = null)
        {
            if (propertyName.IsNullOrEmpty())
            {
                throw new QException("The propertyName is required.");
            }
            string key = $"{GetType().FullName}.{propertyName}";
            return Ls.GetValueOrDefault<TValue>(key);
        }

        protected virtual TValue GetLsOrDefault<TValue>(TValue defaultValue, [CallerMemberName] string propertyName = null)
        {
            if (propertyName.IsNullOrEmpty())
            {
                throw new QException("The propertyName is required.");
            }
            string key = $"{GetType().FullName}.{propertyName}";
            return Ls.GetValueOrDefault<TValue>(key, defaultValue);
        }
        #endregion
    }
}

