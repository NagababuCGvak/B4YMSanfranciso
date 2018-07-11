using System;
using Tar.Logging.Serialization;

namespace Tar.Logging
{
    /// <summary/>
    public static class ExceptionExtensions
    {
        /// <summary/>
        public static string ToXmlString(this Exception exception)
        {
            return ExceptionSerializerFactory.GetSerializer(ExceptionSerializerType.Xml).Serialize(exception);
        }

        /// <summary/>
        public static string ToJsonString(this Exception exception)
        {
            return ExceptionSerializerFactory.GetSerializer(ExceptionSerializerType.Json).Serialize(exception);
        }
    }
}
