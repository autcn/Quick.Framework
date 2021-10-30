using Quick;

namespace ConfigurationDemo
{
    [QConfigurationSection("app")]
    public class AppConfig
    {
        public string LastClickTime { get; set; }
        public string Name { get; set; }
    }
    /*说明：任何一个类，标记了QConfigurationSection特性后, 程序退出时，将自动保存到配置文件，程序启动时，自动加载。
     *     需要注意的是，如果程序崩溃，没有正常退出，配置将丢失，如果需要立即将配置存盘，需要注入IQConfiguration服务, 调用其Save方法
     */
}
