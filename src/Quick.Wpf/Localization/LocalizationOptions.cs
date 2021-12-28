using System;
using System.Collections.Generic;
using System.Reflection;

namespace Quick
{
    public class LocalizationOptions
    {
        public LocalizationOptions()
        {
            _items = new List<LocalizationOptionItem>();
        }

        private List<LocalizationOptionItem> _items;
        public IReadOnlyList<LocalizationOptionItem> Items => _items;

        public void AddFile(string assemblyName, string fileName)
        {
            LocalizationOptionItem item = new LocalizationOptionItem(assemblyName, fileName);
            _items.Add(item);
        }

        public void AddFile(IQModule module, string fileName)
        {
            LocalizationOptionItem item = new LocalizationOptionItem(module.GetType().Assembly.GetName().Name, fileName);
            _items.Add(item);
        }
    }

    public class LocalizationOptionItem
    {
        public LocalizationOptionItem(string assName, string fileName)
        {
            AssemblyName = assName;
            FileName = fileName;
        }
        public string AssemblyName { get; }
        public string FileName { get; }
    }
}
