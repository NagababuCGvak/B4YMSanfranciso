using System;
using System.Collections.Generic;
using System.Linq;
using Tar.Logging.Serialization;

namespace Tar.Logging.Repositories
{
    public class MultipleLogRepository : LogRepository
    {
        private readonly IEnumerable<ILogRepository> _loggers;

        public MultipleLogRepository(IEnumerable<ILogRepository> loggers)
            : base(new DefaultMessageSerializer())
        {
            if (loggers == null)
                throw new ArgumentNullException("loggers");
            _loggers = loggers;
        }

        #region Overrides of LogRepository

        public override void DoLog(IWriteToLogParameter parameter)
        {
            _loggers.ToList().ForEach(logger => logger.WriteToLog(parameter));
        }

        #endregion
    }
}