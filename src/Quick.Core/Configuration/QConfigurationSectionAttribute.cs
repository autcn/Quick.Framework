using System;
using System.Collections.Generic;
using System.Text;

namespace Quick
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class QConfigurationSectionAttribute : Attribute
    {
        public QConfigurationSectionAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; }

        public bool EnableAutoLoad { get; set; } = true;
    }
}
