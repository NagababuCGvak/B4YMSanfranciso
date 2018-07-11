using System;

namespace Tar.Core.LoggingOld2
{
    static class LogMan
    {
        private static readonly IObjectContainer Container = new EnvironmentObjectContainer();
        private static readonly Object LockObject = new Object();
        private const string DefaultLogFactoryKey = "ILoggerFactory";

        public static ILoggerFactory GetLoggerFactory()
        {
            lock (LockObject)
            {
                var factory = Container.Get(DefaultLogFactoryKey) as ILoggerFactory;

                if (factory == null)
                {
                    factory = new LoggerFactory();
                    Container.Set(DefaultLogFactoryKey, factory);
                }
                return factory;
            }
        }

        public static ILogger GetLogger()
        {
            return GetLoggerFactory().GetLogger();
        }

        public static ILogger GetLogger(string name)
        {
            return GetLoggerFactory().GetLogger(name);
        }

        public static ILogScope NewScope(string processName)
        {
            return new LogScope(processName);
        }
        public static ILogScope NewScope(string processName, ILogger logger)
        {
            return new LogScope(processName, logger);
        }

        public static ILogger NewLogger(Func<ILoggerSyntax, ILoggerSyntax> func)
        {
            return func(new LoggerSyntax()).Build();
        }
    }
}
