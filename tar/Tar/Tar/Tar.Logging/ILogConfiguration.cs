using System;
using System.Collections.Generic;
using Tar.Logging.Serialization;

namespace Tar.Logging
{
    public interface ILogConfiguration
    {
        bool IsActive(string process, Type source, LogLevel level);
        Dictionary<string, string> Settings { get; }
        IMessageSerializer Serializer { get; }
    }
}