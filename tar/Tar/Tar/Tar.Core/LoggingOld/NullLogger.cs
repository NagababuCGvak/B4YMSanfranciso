namespace Tar.Core.LoggingOld
{
    public class NullLogger : Logger
    {
        public static ILogger Instance;
        
        static NullLogger()
        {
            Instance = new NullLogger();
        }

        protected override void SendToLog(string source, LogType logType, string message)
        {
        }
    }
}