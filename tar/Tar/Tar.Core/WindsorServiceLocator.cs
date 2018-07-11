using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using Tar.Core.Windsor.ComponentActivator;

namespace Tar.Core
{
    public class WindsorServiceLocator : ServiceLocatorBase
    {
        private readonly IWindsorContainer _container;
        private static bool _traceIsActive = false;

        public WindsorServiceLocator(IWindsorContainer container)
        {
            Trace.WriteLine("Initializing...", "WindsorServiceLocator");
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
            _container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel, true));
            _container.Kernel.Resolver.AddSubResolver(new ListResolver(container.Kernel, true));
            _container.Register(Component.For<IServiceLocator>().Instance(this));

            if (_traceIsActive)
            {
                _container.Kernel.ComponentCreated += (model, instance) =>
                {
                    Trace.WriteLine(string.Format("ComponentCreated|{0}#{1}|{2}|{3}",
                        model.ComponentName,
                        instance.GetHashCode(),
                        model.Implementation,
                        model.LifestyleType), "WindsorServiceLocator");
                };

                _container.Kernel.ComponentDestroyed += (model, instance) =>
                {
                    Trace.WriteLine(string.Format("ComponentDestroyed|{0}#{1}",
                        model.ComponentName,
                        instance.GetHashCode()), "WindsorServiceLocator");
                };
                _container.Kernel.ComponentModelCreated += model =>
                {
                    Trace.WriteLine(
                        string.Format("ComponentModelCreated|{0}|{1}|{2}", model.ComponentName, model.Implementation,
                            model.LifestyleType), "WindsorServiceLocator");
                };
                //_container.Kernel.DependencyResolving += (clientComponentModel, dependencyModel, dependency) =>
                //{
                //    Trace.WriteLineIf(traceIsActive,string.Format(
                //        "DependencyResolving|ClientComponentName:{0}|ReferencedComponentName:{1}|dependency:{2}",
                //        clientComponentModel.ComponentName, dependencyModel.ReferencedComponentName, dependency), "WindsorServiceLocator");
                //};
                //_container.Kernel.ComponentRegistered += (key, handler) =>
                //{
                //    Trace.WriteLineIf(traceIsActive,string.Format(
                //        "ComponentRegistered|Key:{0}|Handler:{1}",
                //        key, handler), "WindsorServiceLocator");
                //};

                //_container.Kernel.RegistrationCompleted += (sender, e) =>
                //{
                //    Trace.WriteLineIf(traceIsActive,string.Format(
                //        "RegistrationCompleted|sender:{0}-e:{1}",
                //        sender, e), "WindsorServiceLocator");
                //};
            }
            Trace.WriteLine("Initialized.", "WindsorServiceLocator");
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
            //RegisterSelf();
            return _container.ResolveAll(service).Cast<object>().ToArray();
        }

        //private bool _isRegistered;
        private readonly List<Type> _autoRegisteredTypes = new List<Type>();

        public override object Get(Type service)
        {
            if (!service.IsClass) return _container.Resolve(service);

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
            //RegisterSelf();
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
                             BuildUpComponentActivator.Key, component
                         }
                     }
                );
        }

        public override void AddConfig(string configFile)
        {
            _container.Install(new ConfigurationInstaller(new XmlInterpreter(configFile)));
        }

        //private void RegisterSelf()
        //{
        //    lock (this)
        //    {
        //        if (_isRegistered) return;
        //        _container
        //            .Register(Component.For<IServiceLocator>()
        //                          .Instance(this));
        //        _isRegistered = true;
        //    }
        //}

        public override object Get(string key, Type service)
        {
            return _container.Resolve(key, service);
        }
    }
}