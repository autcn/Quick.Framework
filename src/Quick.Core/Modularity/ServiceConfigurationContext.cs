using Autofac;
using System.Collections.Generic;

namespace Quick
{
    public class ServiceConfigurationContext
    {
        public ContainerBuilder ServiceBuilder { get; }

        public IDictionary<string, object> Items { get; }

        /// <summary>
        /// Gets/sets arbitrary named objects those can be stored during
        /// the service registration phase and shared between modules.
        ///
        /// This is a shortcut usage of the <see cref="Items"/> dictionary.
        /// Returns null if given key is not found in the <see cref="Items"/> dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get => Items.GetOrDefault(key);
            set => Items[key] = value;
        }

        public ServiceConfigurationContext(ContainerBuilder serviceBuilder)
        {
            ServiceBuilder = serviceBuilder;
            Items = new Dictionary<string, object>();
        }
    }
}