using System;
using System.Diagnostics;
using System.Text;
using Tar.Logging.Serialization;

namespace Tar.Logging.Repositories
{
    public class EventLogRepository : LogRepository
    {
        private readonly string _source;

        public EventLogRepository(string source)
            : base(new DefaultMessageSerializer())
        {
            if (source == null) throw new ArgumentNullException("source");
            _source = source;
        }

        public override void DoLog(IWriteToLogParameter parameter)
        {
            var log = new StringBuilder()
                .AppendLine(string.Format("ActiveUserName:{0}", parameter.ActiveUserName))
                .AppendLine(string.Format("AppFolder:{0}", parameter.AppFolder))
                .AppendLine(string.Format("AppName:{0}", parameter.AppName))
                .AppendLine(string.Format("AssemblyName:{0}", parameter.AssemblyName))
                .AppendLine(string.Format("BuildMode:{0}", parameter.BuildMode))
                .AppendLine(string.Format("ClassName:{0}", parameter.ClassName))
                .AppendLine(string.Format("DateTime:{0}", parameter.DateTime))
                .AppendLine(string.Format("IpAddress:{0}", parameter.IpAddress))
                .AppendLine(string.Format("IsWebApplication:{0}", parameter.IsWebApplication))
                .AppendLine(string.Format("Level:{0}", parameter.Level))
                .AppendLine(string.Format("Process:{0}", parameter.Process))
                .AppendLine(string.Format("ProcessCode:{0}", parameter.ProcessCode))
                .AppendLine(string.Format("ScopeLevel:{0}", parameter.ScopeLevel))
                .AppendLine(string.Format("SerializedMessage:{0}", parameter.SerializedMessage));
            var entryType = parameter.Level == "Error"
                ? EventLogEntryType.Error
                : parameter.Level == "Warn"
                    ? EventLogEntryType.Warning
                    : parameter.Level == "Fatal"
                        ? EventLogEntryType.Error
                        : EventLogEntryType.Information;
            EventLog.WriteEntry(_source, log.ToString(), entryType);
        }
    }
}
