using System;

namespace Quick
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QEditObjectDescriptionAttribute : Attribute
    {
        public QEditObjectDescriptionAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}
