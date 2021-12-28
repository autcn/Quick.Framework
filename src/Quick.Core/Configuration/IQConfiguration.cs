using System;

namespace Quick
{
    public interface IQConfiguration
    {
        T GetConfig<T>(string sectionName) where T : class;
        object GetConfig(Type type, string sectionName);
        T GetOrCreateConfig<T>(string sectionName) where T : class;
        object GetOrCreateConfig(Type type, string sectionName);
        void SaveConfig(string sectionName, object configObj);
        void Save();
    }
}
