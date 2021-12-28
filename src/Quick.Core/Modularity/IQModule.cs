namespace Quick
{
    public interface IQModule
    {
        void PreConfigureServices(ServiceConfigurationContext context);
        void ConfigureServices(ServiceConfigurationContext context);
        void PostConfigureServices(ServiceConfigurationContext context);
        void OnPreApplicationInitialization(ApplicationInitializationContext context);
        void OnApplicationInitialization(ApplicationInitializationContext context);
        void OnPostApplicationInitialization(ApplicationInitializationContext context);
        void OnApplicationShutdown(ApplicationShutdownContext context);
    }
}
