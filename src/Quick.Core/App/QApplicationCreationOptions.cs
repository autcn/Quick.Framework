using Autofac;
using System;

namespace Quick
{
    /// <summary>
    /// 应用程序创建选项
    /// </summary>
    public class QApplicationCreationOptions
    {
        /// <summary>
        /// IOC容器构造器
        /// </summary>
        public ContainerBuilder ServiceBuilder { get; }

        /// <summary>
        /// 配置选项
        /// </summary>
        public QConfigurationBuilderOptions Configuration { get; }

        public QApplicationCreationOptions(ContainerBuilder serviceBuilder)
        {
            ServiceBuilder = serviceBuilder;
            Configuration = new QConfigurationBuilderOptions();
            Configuration.BasePath = AppDomain.CurrentDomain.BaseDirectory;
            Configuration.CommandLineArgs = Environment.GetCommandLineArgs();
        }
    }
}
