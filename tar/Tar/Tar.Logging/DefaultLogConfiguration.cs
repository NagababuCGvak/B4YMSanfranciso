using System;
using System.Collections.Generic;
using Tar.Logging.Serialization;

namespace Tar.Logging
{
    public class DefaultLogConfiguration : ILogConfiguration
    {
        public bool IsActive(string process, Type source, LogLevel level)
        {
            return true;
        }

        public Dictionary<string, string> Settings { get; private set; }
        public IMessageSerializer Serializer { get; private set; }

        public DefaultLogConfiguration()
        {
            Settings = new Dictionary<string, string>();
            Serializer = new DefaultMessageSerializer();
        }
    }
}