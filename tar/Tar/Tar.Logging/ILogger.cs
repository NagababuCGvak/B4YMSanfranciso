using System;

namespace Tar.Logging
{
    public interface ILogger
    {
        string Process();
        ILogger Process(string process);
        string ProcessCode();
        ILogger ProcessCode(string process);

        int ScopeLevel();
        ILogger ScopeLevel(int scopeLevel);

        ILogger Write(Type source, LogLevel level, object message);
        ILogConfiguration Configuration { get; }
        ILogRepository Repository { get; set; }

        ILogScope CreateScope(string scope);
    }
}