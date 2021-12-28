using Autofac;
using Autofac.Builder;
using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Quick
{
    internal class ModuleLoader
    {
        public IQModuleDescriptor[] LoadModules(ContainerBuilder serviceBuilder, ObjectAccessor<IServiceProvider> serviceProviderAccessor, Type startupModuleType)
        {

            var modules = GetDescriptors(serviceBuilder, startupModuleType);

            modules = SortByDependency(modules, startupModuleType);
            ConfigureServices(modules, serviceProviderAccessor, serviceBuilder);

            return modules.ToArray();
        }

        private List<IQModuleDescriptor> GetDescriptors(
            ContainerBuilder serviceBuilder,
            Type startupModuleType)
        {
            var modules = new List<QModuleDescriptor>();

            FillModules(modules, serviceBuilder, startupModuleType);
            SetDependencies(modules);

            return modules.Cast<IQModuleDescriptor>().ToList();
        }

        protected virtual void FillModules(
            List<QModuleDescriptor> modules,
            ContainerBuilder serviceBuilder,
            Type startupModuleType)
        {
            //All modules starting from the startup module
            foreach (var moduleType in QModuleHelper.FindAllModuleTypes(startupModuleType))
            {
                modules.Add(CreateModuleDescriptor(serviceBuilder, moduleType));
            }
        }

        protected virtual void SetDependencies(List<QModuleDescriptor> modules)
        {
            foreach (var module in modules)
            {
                SetDependencies(modules, module);
            }
        }

        protected virtual List<IQModuleDescriptor> SortByDependency(List<IQModuleDescriptor> modules, Type startupModuleType)
        {
            var sortedModules = modules.SortByDependencies(m => m.Dependencies);
            sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
            return sortedModules;
        }

        protected virtual QModuleDescriptor CreateModuleDescriptor(ContainerBuilder serviceBuilder, Type moduleType, bool isLoadedAsPlugIn = false)
        {
            return new QModuleDescriptor(moduleType, CreateAndRegisterModule(serviceBuilder, moduleType), isLoadedAsPlugIn);
        }

        protected virtual IQModule CreateAndRegisterModule(ContainerBuilder serviceBuilder, Type moduleType)
        {
            var module = (IQModule)Activator.CreateInstance(moduleType);
            serviceBuilder.RegisterInstance(module).As(moduleType).SingleInstance();
            return module;
        }

        protected virtual void ConfigureServices(List<IQModuleDescriptor> modules, ObjectAccessor<IServiceProvider> serviceProviderAccessor, ContainerBuilder serviceBuilder)
        {
            var context = new ServiceConfigurationContext(serviceBuilder);
            serviceBuilder.RegisterInstance(context).SingleInstance();

            foreach (var module in modules)
            {
                if (module.Instance is QModule sisModule)
                {
                    sisModule.ServiceConfigurationContext = context;
                }
            }

            //PreConfigureServices
            foreach (var module in modules.Where(m => m.Instance is QModule))
            {
                module.Instance.PreConfigureServices(context);
            }

            //ConfigureServices
            //defined a hashset to prevent adding same assembly twice.
            HashSet<Assembly> assemblyHashSet = new HashSet<Assembly>();
            foreach (var module in modules)
            {
                if (!assemblyHashSet.Contains(module.Type.Assembly))
                {
                    RegisterAutoConfig(serviceBuilder, module);
                    RegisterConventionalServices(serviceBuilder, module.Type.Assembly);
                    assemblyHashSet.Add(module.Type.Assembly);
                }
                module.Instance.ConfigureServices(context);
            }

            //Register AutoMapper
            RegisterAutoMapper(serviceBuilder, serviceProviderAccessor, assemblyHashSet);

            //PostConfigureServices
            foreach (var module in modules.Where(m => m.Instance is QModule))
            {
                module.Instance.PostConfigureServices(context);
            }

            foreach (var module in modules)
            {
                if (module.Instance is QModule sisModule)
                {
                    sisModule.ServiceConfigurationContext = null;
                }
            }
        }

        protected virtual void RegisterAutoMapper(ContainerBuilder serviceBuilder, ObjectAccessor<IServiceProvider> serviceProviderAccessor, IEnumerable<Assembly> assemblies)
        {
            MapperConfigurationExpression autoMapperConfiguration = new MapperConfigurationExpression();
            autoMapperConfiguration.AddMaps(assemblies);
            autoMapperConfiguration.ConstructServicesUsing(type => serviceProviderAccessor.Value.GetService(type));
            var autoMapperConfig = new MapperConfiguration(autoMapperConfiguration);
            serviceBuilder.Register(c => autoMapperConfig.CreateMapper()).As<IMapper>();
        }

        protected virtual void RegisterAutoConfig(ContainerBuilder serviceBuilder, IQModuleDescriptor module)
        {
            var types = module.Type.Assembly.GetTypes().Where(p => p.IsDefined(typeof(QConfigurationSectionAttribute), false));
            foreach (Type type in types)
            {
                var attr = type.GetCustomAttribute<QConfigurationSectionAttribute>();
                if (attr.EnableAutoLoad)
                    (module.Instance as QModule).Configure(type);
            }
        }

        protected virtual void RegisterConventionalServices(ContainerBuilder serviceBuilder, Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                {
                    continue;
                }
                List<Attribute> attrs = type.GetCustomAttributes().ToList();
                attrs.AddRange(type.GetInterfaces().SelectMany(p => p.GetCustomAttributes()));

                bool propertiesAutowired = false;
                bool hasInjectAttr = false;
                bool isSingleton = false;
                foreach (Attribute attr in attrs.OrderBy(p => (p is SingletonDependencyAttribute) ? 0 : 1))
                {
                    if (attr is SingletonDependencyAttribute singleTonAttr)
                    {
                        isSingleton = true;
                        hasInjectAttr = true;
                        propertiesAutowired = singleTonAttr.PropertiesAutowired;
                        break;
                    }
                    if (attr is TransientDependencyAttribute transientAttr)
                    {
                        isSingleton = false;
                        hasInjectAttr = true;
                        propertiesAutowired = transientAttr.PropertiesAutowired;
                        break;
                    }
                }
                if (!hasInjectAttr)
                {
                    continue;
                }
                ExposeServicesAttribute exposedServiceAttr = type.GetCustomAttribute<ExposeServicesAttribute>(false);
                string exposeName = null;
                List<Type> exposedTypes = new List<Type>();
                if (exposedServiceAttr == null)
                {
                    //添加自己
                    exposedTypes.Add(type);
                    //找同名接口
                    Type interfaceType = type.GetInterface("I" + type.Name);
                    if (interfaceType != null)
                    {
                        exposedTypes.Add(interfaceType);
                    }
                }
                else
                {
                    exposeName = exposedServiceAttr.Name;
                    exposedTypes = exposedServiceAttr.GetExposedServiceTypes(type).ToList();
                }

                var register = serviceBuilder.RegisterType(type);
                foreach (var exposeType in exposedTypes)
                {
                    if (exposeName == null)
                    {
                        register.As(exposeType);
                    }
                    else
                    {
                        register.Named(exposeName, exposeType);
                    }
                }
                if (propertiesAutowired)
                {
                    register.PropertiesAutowired();
                }
                if (isSingleton)
                {
                    register.SingleInstance();
                }
                register.Initializable();
            }
        }

        protected virtual void SetDependencies(List<QModuleDescriptor> modules, QModuleDescriptor module)
        {
            foreach (var dependedModuleType in QModuleHelper.FindDependedModuleTypes(module.Type))
            {
                var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new QException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
                }

                module.AddDependency(dependedModule);
            }
        }
    }
}