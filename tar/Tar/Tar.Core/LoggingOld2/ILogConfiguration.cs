using System;
using System.Collections.Generic;
using Tar.Core.LoggingOld2.Serialization;

namespace Tar.Core.LoggingOld2
{
    public interface ILogConfiguration
    {
        bool IsActive(string process, Type source, LogLevel level);
        Dictionary<string, string> Settings { get; }
        IMessageSerializer Serializer { get; }
    }
}