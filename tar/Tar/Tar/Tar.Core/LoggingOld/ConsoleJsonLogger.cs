using System;
using Tar.Core.Extensions;

namespace Tar.Core.LoggingOld
{
    public class ConsoleJsonLogger : Logger
    {
        protected override void SendToLog(string source, LogType logType, string message)
        {
            Console.WriteLine(new { LogCategory = source, LogType = logType.ToString(), LogMessage = message }.ToJson());
        }
    }
}
