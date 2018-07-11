namespace Tar.Core.LoggingOld2
{
    interface ILoggerFactory
    {
        ILogger GetLogger();
        ILogger GetLogger(string name);
    }
}