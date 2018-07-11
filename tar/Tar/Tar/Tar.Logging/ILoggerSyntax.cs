namespace Tar.Logging
{
    public interface ILoggerSyntax
    {
        ILoggerSyntax Configuration(ILogConfiguration config);
        ILogConfiguration Configuration();
        ILoggerSyntax Repository(ILogRepository repository);
        ILogRepository Repository();
        ILogger Build();
    }
}