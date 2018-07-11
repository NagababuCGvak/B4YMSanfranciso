using System.Collections.Generic;
using Tar.Core.LoggingOld2.Repositories;
using Tar.Core.LoggingOld2.Serialization;

namespace Tar.Core.LoggingOld2
{
    class LoggerFactory : ILoggerFactory
    {
        private readonly Dictionary<string, ILogger> _loggers;
        #region Implementation of ILoggerFactory
        ILogger ILoggerFactory.GetLogger()
        {
            return ((ILoggerFactory)this).GetLogger("default");
        }

        ILogger ILoggerFactory.GetLogger(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = "default";

            name = name.ToLower();

            if (!_loggers.ContainsKey(name))
            {
                var connectionStringName = "DefaultConnection";
                var dataAccess = new LogDbDataAccess(connectionStringName);
                IMessageSerializer serializer = new DefaultMessageSerializer();

                var repository = new DbLogRepository(serializer,dataAccess)
                                     {
                                         NextRepository = new FileLogRepository(serializer, "c:\\FileLogRepository.log")
                                                              {
                                                                  NextRepository = new ConsoleLogRepository(serializer)
                                                              }
                                     };

                var logger = LogMan.NewLogger(l =>
                                              l.Configuration(
                                                  new DbLogConfiguration(connectionStringName, serializer, dataAccess))
                                                  .Repository(repository));

                _loggers.Add(name, logger);
            }
            return _loggers[name];
        }
        #endregion

        public LoggerFactory()
        {
            _loggers = new Dictionary<string, ILogger>();
        }
    }
}