namespace Quick
{
    /// <summary>
    /// 应用程序配置选项
    /// </summary>
    public class QConfigurationBuilderOptions
    {
        /// <summary>
        /// 配置文件名称，默认为appsettings.json
        /// </summary>
        public string ConfigFileName { get; set; } = "appsettings.json";

        /// <summary>
        /// 本地存储文件名称，默认为"localstorage.json"，如果设置为空，则本地存储合并到配置文件中
        /// </summary>
        public string LocalStorageFileName { get; set; } = "localstorage.json";

        /// <summary>
        /// 环境名称
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// 数据存储基础目录，默认为程序启动目录
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// 程序启动参数
        /// </summary>
        public string[] CommandLineArgs { get; set; }
    }
}
