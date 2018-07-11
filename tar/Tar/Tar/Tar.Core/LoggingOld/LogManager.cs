using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Tar.Core.LoggingOld.Configuration;

namespace Tar.Core.LoggingOld
{
    public static class LogManager
    {
        public static ILogger Default { get; private set; }

        public static void SetLogger(ILogger logger)
        {
            Default = logger;
        }

        public static void Initialize()
        {
            Initialize("zahir/logging");
        }

        public static List<LoggerConfigurationElement> LoggerElements;
        public static LoggingConfigurationSection Section;
        internal static Dictionary<string, ILogger> Loggers;

        public static void Initialize(string sectionName)
        {
            Loggers = new Dictionary<string, ILogger>();
            
            Section = (LoggingConfigurationSection)ConfigurationManager.GetSection(sectionName);
            LoggerElements = Section.Loggers.ToList();

            LoggerElements.ForEach(e => Loggers.Add(e.Name, NewLogger(e.Name)));

            Default = GetLogger(Section.Default);
        }

        public static ILogger GetLogger(string loggerName)
        {
            if (!Loggers.ContainsKey(loggerName))
                throw new Exception(string.Format("Logger bulunamadı! LoggerName: {0}", loggerName));

            return Loggers.First(l => l.Key == loggerName).Value;
        }

        public static ILogger NewLogger(string loggerName)
        {
            var loggerElement = LoggerElements.FirstOrDefault(l => l.Name == loggerName);

            if (loggerElement == null)
                throw new ConfigurationErrorsException("default logger tanımlı değil yada bulunamadı!");

            var constructorParameterElements = loggerElement.ConstructorParameters.ToList();

            var parameters = new object[] { };

            if (constructorParameterElements.Count > 0)
            {
                var tmp = new List<object>();
                constructorParameterElements.ForEach(c => tmp.Add(c.Value));
                parameters = tmp.ToArray();
            }

            var type = Type.GetType(loggerElement.Type);

            var constructorInfos = type.GetConstructors().Where(c => c.GetParameters().Count() == parameters.Count());

            var constructor = constructorInfos.Where(
                c => c.GetParameters().Select(p => p.Name).All(
                    p => constructorParameterElements.Select(c2 => c2.Name).
                             Contains(p))).FirstOrDefault();

            if (constructor == null)
                throw new Exception("Constructor yakalanamadı!");

            var logger = (ILogger)constructor.Invoke(parameters);
            logger.Category(loggerName);
            if (!string.IsNullOrEmpty(loggerElement.NextLogger))
                logger.NextLogger(NewLogger(loggerElement.NextLogger));

            return logger;
        }
    }
}
