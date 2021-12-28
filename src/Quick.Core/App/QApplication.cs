using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quick
{
    public class QApplication
    {
        public IServiceProvider ServiceProvider => _serviceProviderAccessor.Value;
        public IContainer ServiceContainer => _serviceContainerAccessor.Value;
        public ContainerBuilder ServiceBuilder { get; }
        public Type StartupModuleType { get; }
        public QApplicationCreationOptions CreationOptions { get; }
        public IReadOnlyList<IQModuleDescriptor> Modules { get; }
        private ObjectAccessor<IServiceProvider> _serviceProviderAccessor;
        private ObjectAccessor<IContainer> _serviceContainerAccessor;
        public QApplication(Type startupModuleType, Action<QApplicationCreationOptions> optionsAction)
        {
            _serviceProviderAccessor = new ObjectAccessor<IServiceProvider>();
            _serviceContainerAccessor = new ObjectAccessor<IContainer>();
            ServiceBuilder = new ContainerBuilder();
            ServiceBuilder.Register(p => _serviceContainerAccessor.Value).As<IContainer>().SingleInstance();
            ServiceBuilder.Register(p => _serviceProviderAccessor.Value).As<IServiceProvider>().SingleInstance();
            StartupModuleType = startupModuleType;
            var options = new QApplicationCreationOptions(ServiceBuilder);
            optionsAction?.Invoke(options);
            CreationOptions = options;
            AddCoreServices();
            Modules = LoadModules();
            _serviceContainerAccessor.Value = ServiceBuilder.Build();
            _serviceProviderAccessor.Value = new ServiceProviderImp(ServiceContainer);
        }

        #region Privte functions
        private void AddCoreServices()
        {
            AddConfiguration();
            AddLogger();
            AddLocalStorage();
            AddText();
        }

        private void AddConfiguration()
        {
            string configFilePath = Path.Combine(CreationOptions.Configuration.BasePath, CreationOptions.Configuration.ConfigFileName);
            ServiceBuilder.AddConfiguration(configFilePath);
        }

        private void AddText()
        {
            ServiceBuilder.AddText();
        }
        private void AddLogger()
        {
            ServiceBuilder.Configure<QLoggerConfiguration>();
            ServiceBuilder.Register(p =>
            {
                QLoggerConfiguration qLogCfg = p.Resolve<QLoggerConfiguration>();
                if (qLogCfg.SinkTypes.IsNullOrEmpty())
                {
                    qLogCfg.SinkTypes = new List<LoggerSinkType>();
                    qLogCfg.SinkTypes.Add(LoggerSinkType.Console);
                    qLogCfg.SinkTypes.Add(LoggerSinkType.File);
                }

                var loggerConfig = new LoggerConfiguration();
                loggerConfig.MinimumLevel.Is(qLogCfg.ConsoleLevel < qLogCfg.FileLevel ? qLogCfg.ConsoleLevel : qLogCfg.FileLevel);

                if (qLogCfg.SinkTypes.Contains(LoggerSinkType.Console))
                {
                    loggerConfig.WriteTo.Console(qLogCfg.ConsoleLevel, qLogCfg.OutputTemplate);
                }

                if (qLogCfg.SinkTypes.Contains(LoggerSinkType.File))
                {
                    string logFileName = null;
                    if (!qLogCfg.LogStorageDir.IsNullOrEmpty())
                    {
                        logFileName = Path.Combine(qLogCfg.LogStorageDir, qLogCfg.FileName);
                    }
                    else
                    {
                        logFileName = Path.Combine(CreationOptions.Configuration.BasePath, QProperties.LogDefaultStorageDir, qLogCfg.FileName);
                    }
                    loggerConfig.WriteTo.File(path: logFileName, restrictedToMinimumLevel: qLogCfg.FileLevel,
                        outputTemplate: qLogCfg.OutputTemplate, rollingInterval: qLogCfg.RollingInterval);
                }

                return loggerConfig.CreateLogger();
            }).As<ILogger>().SingleInstance();
        }

        private void AddLocalStorage()
        {
            string localStorageFilePath = null;
            if (!CreationOptions.Configuration.LocalStorageFileName.IsNullOrEmpty())
            {
                localStorageFilePath = Path.Combine(CreationOptions.Configuration.BasePath, CreationOptions.Configuration.LocalStorageFileName);
            }
            ServiceBuilder.AddLocalStorage(localStorageFilePath);
        }

        private IQModuleDescriptor[] LoadModules()
        {
            ModuleLoader loader = new ModuleLoader();
            return loader.LoadModules(ServiceBuilder, _serviceProviderAccessor, StartupModuleType);
        }

        private void InitializeModules()
        {
            ApplicationInitializationContext context = new ApplicationInitializationContext(ServiceProvider);
            foreach (IQModuleDescriptor qModule in Modules)
            {
                qModule.Instance.OnPreApplicationInitialization(context);
            }

            foreach (IQModuleDescriptor qModule in Modules)
            {
                qModule.Instance.OnApplicationInitialization(context);
            }

            foreach (IQModuleDescriptor qModule in Modules)
            {
                qModule.Instance.OnPostApplicationInitialization(context);
            }
        }
        #endregion

        #region Public functions
        public void Initialize()
        {
            InitializeModules();
        }

        public void Shutdown()
        {
            ApplicationShutdownContext context = new ApplicationShutdownContext(ServiceProvider);
            foreach (IQModuleDescriptor qModule in Modules.Reverse())
            {
                qModule.Instance.OnApplicationShutdown(context);
            }
            ServiceProvider.GetService<IQConfiguration>().Save();
            ServiceProvider.GetService<ILocalStorage>().Save();
            _serviceContainerAccessor.Value.Dispose();
        }
        #endregion
    }
}
