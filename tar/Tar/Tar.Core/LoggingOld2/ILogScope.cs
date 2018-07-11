using System;

namespace Tar.Core.LoggingOld2
{
    public interface ILogScope : IDisposable
    {
        string ProcessName { get; }
    }
}
