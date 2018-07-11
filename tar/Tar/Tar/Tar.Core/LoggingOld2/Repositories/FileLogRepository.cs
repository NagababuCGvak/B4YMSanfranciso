using System.IO;
using System.Text;
using Tar.Core.LoggingOld2.Serialization;

namespace Tar.Core.LoggingOld2.Repositories
{
    class FileLogRepository : LogRepository
    {
        private readonly string _filePath;

        
        #region Implementation of ILogRepository

        public FileLogRepository(IMessageSerializer serializer, string filePath) : base(serializer)
        {
            _filePath = filePath;
        }

        private const string Line = "------------------------------------";

        public override void Log(IWriteToLogParameter parameter)
        {
            using (var sw =new StreamWriter(_filePath, true,Encoding.UTF8))
            {
                var log = new StringBuilder()
                    .AppendLine(Line)
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
                    .AppendLine(string.Format("SerializedMessage:{0}", parameter.SerializedMessage))
                    .AppendLine();
                sw.WriteLine(log);
            }
        }
        #endregion
    }
}