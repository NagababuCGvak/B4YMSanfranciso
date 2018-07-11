using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;

namespace Tar.Core
{
    public class WindsorServiceLocator : ServiceLocatorBase
    {
        private readonly IWindsorContainer _container;

        public WindsorServiceLocator(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        public WindsorServiceLocator(string fileName)
            : this(new WindsorContainer(new XmlInterpreter(fileName)))
        {
        }

        public WindsorServiceLocator()
            :this(AppSettings.ServiceLocatorConfigFile)
        {
        }

        public override object[] GetAll(Type service)
        {
            RegisterSelf();
            return _container.ResolveAll(service).Cast<object>().ToArray();
        }

        private bool _isRegistered;
        private readonly List<Type> _autoRegisteredTypes = new List<Type>();

        public override object Get(Type service)
        {
            lock (this)
            {
                if (!_autoRegisteredTypes.Contains(service)
                    && service.IsClass
                    && !_container.Kernel.HasComponent(service))
                {
                    _container.Register(
                        Component.For(service)
                            .LifestyleTransient());
                    _autoRegisteredTypes.Add(service);
                }
                
            }
            RegisterSelf();
            return _container.Resolve(service);
        }

        //public override T BuildUp<T>(T component)
        //{
        //    return _container.Kernel.Resolve<T>
        //        (
        //            new Dictionary<string, T>
        //                {
        //                    {
        //                        Windsor.ComponentActivator.BuildUpComponentActivator.Key, component
        //                    }
        //                }
        //        );
        //}

        public override object BuildUp(Type serviceType, object component)
        {
            return _container.Kernel.Resolve
                (serviceType,
                 new Dictionary<string, object>
                     {
                         {
                             Windsor.ComponentActivator.BuildUpComponentActivator.Key, component
                         }
                     }
                );
        }

        public override void AddConfig(string configFile)
        {
            _container.Install(new ConfigurationInstaller(new XmlInterpreter(configFile)));
        }

        private void RegisterSelf()
        {
            lock (this)
            {
                if (_isRegistered) return;
                _container
                    .Register(Component.For<IServiceLocator>()
                                  .Instance(this));
                _isRegistered = true;
            }
        }

        public override object Get(string key, Type service)
        {
            return _container.Resolve(key, service);
        }
    }
}