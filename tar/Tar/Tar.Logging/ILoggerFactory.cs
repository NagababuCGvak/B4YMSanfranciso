namespace Tar.Logging
{
    public interface ILoggerFactory
    {
        ILogger GetLogger();
        ILogger GetLogger(string name);
    }
}