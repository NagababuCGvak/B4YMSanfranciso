using System.Linq;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace Tar.Core
{
    public static class ServiceLocator
    {
        private static IServiceLocator _current;

        public static IServiceLocator Current
        {
            get
            {
                if (_current == null)
                    Initialize();
                return _current;
            }
            set { _current = value; }
        }

        public static void Initialize(string configFile)
        {
            _current = new WindsorServiceLocator(
                new WindsorContainer(
                    new XmlInterpreter(configFile)));
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