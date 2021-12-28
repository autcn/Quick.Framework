using System;

namespace Quick
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class SingletonDependencyAttribute : Attribute
    {
        public bool PropertiesAutowired { get; set; }
    }
}
