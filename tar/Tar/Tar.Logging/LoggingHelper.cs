using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Tar.Logging.Configuration;
using Tar.Logging.ObjectContainers;

namespace Tar.Logging
{
    public class LoggingHelper
    {
        private static string _sectionName;

        private const string DefaultSectionName = "tar/logging";

        protected static IObjectContainer HelperContainer = new EnvironmentObjectContainer();

        public static LoggingHelper Instance
        {
            get
            {
                //if (_sectionName == null) throw new Exception("You dont call initialize!");
                Initialize();
                if (HelperContainer.Get(_sectionName) == null)
                    HelperContainer.Set(_sectionName, new LoggingHelper(_sectionName));
                return (LoggingHelper) HelperContainer.Get(_sectionName);
            }
        }

        public static void Initialize(string sectionName)
        {
            if (sectionName == null) throw new ArgumentNullException("sectionName");
            _sectionName = sectionName;
        }

        public static void Initialize()
        {
            Initialize(DefaultSectionName);
        }

        private const string DefaultLoggerKey = "defaultLogger";

        public ILogger GetLogger()
        {
            return GetLogger(DefaultLoggerKey);
        }

        public ILogger GetLogger(string loggerName)
        {
            return Loggers.First(logger => logger.Key.ToLowerInvariant() == loggerName.ToLower()).Value;
        }

        protected Dictionary<string, ILogRepository> Repositories { get; private set; }
        protected Dictionary<string, ILogger> Loggers { get; private set; }

        public LoggingHelper(string sectionName)
        {
            if (sectionName == null) throw new ArgumentNullException("sectionName");
            
            Section = (LoggingConfigurationSection) ConfigurationManager.GetSection(sectionName);

            Repositories = new Dictionary<string, ILogRepository>();
            Loggers = new Dictionary<string, ILogger>();

            ReadConfiguration();
        }

        private void ReadConfiguration()
        {
            Section.Repositories.ToList().ForEach(
                repoConfig => Repositories.Add(repoConfig.Name, CreateFrom<ILogRepository>(repoConfig)));
            
            Section.Repositories.ToList().Where(repoConfig => !string.IsNullOrEmpty(repoConfig.NextRepository)).ToList()
                .ForEach(
                    repoConfig => Repositories[repoConfig.Name].NextRepository = Repositories[repoConfig.NextRepository]);

            Section.Loggers.ToList().ForEach(
                loggerConfig =>
                    {
                        var logger = CreateFrom<ILogger>(loggerConfig);
                        logger.Repository = Repositories[loggerConfig.Repository];
                        Loggers.Add(loggerConfig.Name, logger);
                    });
        }

        public static T CreateFrom<T>(TypeConfigurationElement typeConfiguration)
        {
            var constructorParameterElements = typeConfiguration.ConstructorParameters.ToList();

            var parameters = new object[] { };

            if (constructorParameterElements.Count > 0)
            {
                var tmp = new List<object>();
                constructorParameterElements.ForEach(c => tmp.Add(c.Value));
                parameters = tmp.ToArray();
            }

            var type = Type.GetType(typeConfiguration.Type);
            if (type == null)
                throw new Exception(string.Format("'{0}' cannot convert to type. ", typeConfiguration.Type));

            var constructorInfos = type.GetConstructors().Where(c => c.GetParameters().Count() == parameters.Count());

            var constructor = constructorInfos.FirstOrDefault(c => c.GetParameters().Select(p => p.Name).All(
                p => constructorParameterElements.Select(c2 => c2.Name).
                         Contains(p)));

            if (constructor == null)
                throw new Exception("Constructor yakalanamadı!");
            var instance = constructor.Invoke(parameters);
            return (T)instance;
        }

        protected LoggingConfigurationSection Section { get;private set; }
    }
}