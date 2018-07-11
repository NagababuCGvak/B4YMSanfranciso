using System;

namespace Tar.Logging
{
    public interface ILogScope : IDisposable
    {
        string ProcessName { get; }
    }
}
