using System;
using System.Collections.Generic;
using System.Reflection;

namespace Quick
{
    public interface IQModuleDescriptor
    {
        Type Type { get; }

        IQModule Instance { get; }

        IReadOnlyList<IQModuleDescriptor> Dependencies { get; }
    }
}