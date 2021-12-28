using System;

namespace Quick
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class EnumNullDescriptionAttribute : Attribute
    {
        public EnumNullDescriptionAttribute(string description)
        {
            Description = description;
        }
        public string Description { get; }
    }
}
