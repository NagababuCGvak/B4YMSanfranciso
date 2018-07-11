using System.Collections.Generic;
using Tar.Logging.Repositories;
using Tar.Logging.Serialization;

namespace Tar.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        private readonly Dictionary<string, ILogger> _loggers;
        #region Implementation of ILoggerFactory
        ILogger ILoggerFactory.GetLogger()
        {
            return ((ILoggerFactory)this).GetLogger();
        }

        ILogger ILoggerFactory.GetLogger(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = "defaultLogger";

            name = name.ToLower();

            if (!_loggers.ContainsKey(name))
            {
                var connectionStringName = "DefaultConnection";
                var dataAccess = new LogDbDataAccess(connectionStringName);
                IMessageSerializer serializer = new DefaultMessageSerializer();

                var repository = new DbLogRepository(connectionStringName)
                                     {
                                         NextRepository = new FileLogRepository("c:\\FileLogRepository.log")
                                                              {
                                                                  NextRepository = new ConsoleLogRepository()
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