namespace Tar.Core.LoggingOld
{
    public interface ILogger
    {
        ILogger Log(string source, LogType logType, string message);
        ILogger Log(string source, LogType logType, string message, params object[] parameters);
        string Category();
        ILogger Category(string categrory);
        ILogger NextLogger();
        ILogger NextLogger(ILogger logger);
    }
}
