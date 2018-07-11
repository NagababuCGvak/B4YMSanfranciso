namespace Tar.Core.LoggingOld2
{
    interface ILoggerSyntax
    {
        ILoggerSyntax Configuration(ILogConfiguration config);
        ILogConfiguration Configuration();
        ILoggerSyntax Repository(ILogRepository repository);
        ILogRepository Repository();
        ILogger Build();
    }
}