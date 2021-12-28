using System;

namespace Quick
{
    public interface IDependedTypesProvider
    {
        Type[] GetDependedTypes();
    }
}