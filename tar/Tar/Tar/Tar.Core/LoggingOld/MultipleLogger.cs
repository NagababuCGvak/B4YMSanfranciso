using System.Collections.Generic;
using System.Linq;

namespace Tar.Core.LoggingOld
{
    public class MultipleLogger : Logger
    {
        public List<ILogger> Loggers { get; set; }
        protected override void SendToLog(string source, LogType logType, string message)
        {
            Loggers.ForEach(logger => logger.Log(source, logType, message));
        }

        protected MultipleLogger(params ILogger[] loggers)
        {
            Loggers = new List<ILogger>();
            if (loggers != null)
                loggers.ToList().ForEach(Loggers.Add);
        }
    }
}