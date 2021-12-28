using System;
using System.Collections.Generic;
using System.Reflection;

namespace Quick
{
    internal class QModuleDescriptor : IQModuleDescriptor
    {
        public Type Type { get; }

        public IQModule Instance { get; }

        public bool IsLoadedAsPlugIn { get; }

        public IReadOnlyList<IQModuleDescriptor> Dependencies => _dependencies;
        private readonly List<IQModuleDescriptor> _dependencies;

        public QModuleDescriptor(
             Type type, 
             IQModule instance, 
            bool isLoadedAsPlugIn)
        {

            if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
            }

            Type = type;
            Instance = instance;
            IsLoadedAsPlugIn = isLoadedAsPlugIn;

            _dependencies = new List<IQModuleDescriptor>();
        }

        public void AddDependency(IQModuleDescriptor descriptor)
        {
            _dependencies.AddIfNotContains(descriptor);
        }

        public override string ToString()
        {
            return $"[QModuleDescriptor {Type.FullName}]";
        }
    }
}
