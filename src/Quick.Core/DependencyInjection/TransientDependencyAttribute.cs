using System;

namespace Quick
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TransientDependencyAttribute : Attribute
    {
        public bool PropertiesAutowired { get; set; } = false;
    }
}
