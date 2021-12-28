using Autofac;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Quick
{
    /// <summary>
    /// Wpf application增强类
    /// </summary>
    public abstract class QWpfApplication : Application
    {
        #region Constructor

        public QWpfApplication()
        {
            ResourceDictionary converterResDict = new ResourceDictionary();
            converterResDict.Source = new Uri("pack://application:,,,/Quick.Wpf;component/Resources/Converters.xaml");
            Resources.MergedDictionaries.Add(converterResDict);
        }

        #endregion

        #region Private members
        private EventWaitHandle _keepOnceEvent;
        private QApplication _qApplication;
        private ILogger _logger;
        private QLoggerConfiguration _loggerConfig;
        private CancellationTokenSource _cancellationTokenSource;
        #endregion

        #region Properties
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IContainer ServiceContainer { get; private set; }
        public bool QuickShutdown { get; set; } = false;
        public string SingleInstanceAppId { get; set; }
        public bool PreventShutdownWhenCrashed { get; set; } = true;
        public abstract Type StartupModuleType { get; }
        public bool IsShutting { get; private set; } = false;
        public bool ShowPromptWhenStartSecondApp { get; set; } = false;
        #endregion

        #region Overwrite functions

        protected virtual void ConfigureStartup(QApplicationCreationOptions options)
        {

        }

        protected virtual void OnStartupNextInstance()
        {
            if (MainWindow != null)
            {
                MainWindow.Show();
                MainWindow.Activate();
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Topmost = true;
                MainWindow.Topmost = false;
            }
        }
        #endregion

        #region Startup
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!SingleInstanceAppId.IsNullOrEmpty())
            {
                bool isNew = false;
                _keepOnceEvent = new EventWaitHandle(false, EventResetMode.ManualReset, SingleInstanceAppId, out isNew);
                if (!isNew)
                {
                    if (ShowPromptWhenStartSecondApp)
                    {
                        MessageBox.Show("The application is already running.", "Note", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    _keepOnceEvent.Set();//发信号通知主程序
                    TryQuickShutdown();
                    return;
                }
                StartEventWaitTask();
            }

            _qApplication = new QApplication(StartupModuleType, options => ConfigureStartup(options));
            ServiceProvider = _qApplication.ServiceProvider;
            ServiceContainer = _qApplication.ServiceContainer;
            _qApplication.Initialize();
            _logger = ServiceProvider.GetService<ILogger>();
            _loggerConfig = ServiceProvider.GetService<QLoggerConfiguration>();

            base.OnStartup(e);
            DispatcherUnhandledException += QWpfApplication_DispatcherUnhandledException;
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void StartEventWaitTask()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _keepOnceEvent.WaitOne();
                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                    this.Dispatcher.BeginInvoke(new Action(OnStartupNextInstance));
                    _keepOnceEvent.Reset();
                }
            }, _cancellationTokenSource.Token);
        }

        #endregion

        #region Events Handlers
        private void QWpfApplication_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                //崩溃时，尝试保存配置
                if (_qApplication.ServiceProvider.TryGetService(out IQConfiguration config))
                {
                    config.Save();
                }
                //崩溃时，尝试保存配置
                if (_qApplication.ServiceProvider.TryGetService(out ILocalStorage localStorage))
                {
                    localStorage.Save();
                }
            }
            catch { }
            string error = e.Exception.ToString();
            _logger.Error(e.Exception, "The application is crashed!");
            MessageBox.Show(error, "Application Crashed", MessageBoxButton.OK, MessageBoxImage.Error);
            if (_loggerConfig.SinkTypes.Contains(LoggerSinkType.File))
            {
                string logPath = _loggerConfig.LogStorageDir.IsNullOrEmpty() ?
                    Path.Combine(_qApplication.CreationOptions.Configuration.BasePath, QProperties.LogDefaultStorageDir) :
                    _loggerConfig.LogStorageDir;

                MessageBox.Show($"The crash info has been saved to the file that the path is: \"{logPath}\". Please send it to developer.",
                "Application Crashed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (PreventShutdownWhenCrashed)
                e.Handled = true;
            else
                TryQuickShutdown();
        }

        #endregion

        #region Exit  

        protected override void OnExit(ExitEventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _keepOnceEvent.Set();
            }
            if (_qApplication != null)
            {
                ServiceProvider = null;
                _qApplication.Shutdown();
            }
            base.OnExit(e);
            _keepOnceEvent?.Close();
            if (QuickShutdown)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private void TryQuickShutdown()
        {
            if (QuickShutdown)
            {
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                IsShutting = true;
                this.Shutdown();
            }
        }
        #endregion

    }
}
