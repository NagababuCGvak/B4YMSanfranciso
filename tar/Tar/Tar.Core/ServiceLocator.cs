using System;
using System.Linq;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace Tar.Core
{
    public static class ServiceLocator
    {
        private static IServiceLocator _current;
        private static readonly object LockObject = new object();

        private static void OnLock(Func<bool> condition, Action action)
        {
            if (condition())
                lock (LockObject)
                {
                    if (condition())
                        action();
                }
        }

        public static IServiceLocator Current
        {
            get
            {
                OnLock(() => _current == null, Initialize);
                return _current;
            }
            set { _current = value; }
        }

        public static void Initialize(string configFile)
        {
            var container = new WindsorContainer(new XmlInterpreter(configFile));
            _current = new WindsorServiceLocator(container);
            _current.GetAll<IBootStrapper>().ToList().ForEach(item => item.BootStrap());
        }

        public static void Initialize()
        {
            Initialize(AppSettings.ServiceLocatorConfigFile);
        }

        public static void Reset()
        {
            Current = null;
        }
    }
}