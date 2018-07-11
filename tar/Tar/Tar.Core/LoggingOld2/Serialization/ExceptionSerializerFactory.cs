using System;
using System.Collections.Generic;

namespace Tar.Core.LoggingOld2.Serialization
{
    /// <summary/>
    public static class ExceptionSerializerFactory
    {
        private static readonly Dictionary<ExceptionSerializerType, IExceptionSerializer> ExceptionSerializers;

        static ExceptionSerializerFactory()
        {
            ExceptionSerializers = new Dictionary<ExceptionSerializerType, IExceptionSerializer>();
        }

        /// <summary/>
        public static IExceptionSerializer GetSerializer(ExceptionSerializerType serializerType)
        {
            if (!ExceptionSerializers.ContainsKey(serializerType))
            {
                ExceptionSerializers[serializerType] = CreateSerializer(serializerType);
            }

            return ExceptionSerializers[serializerType];
        }

        /// <summary/>
        public static IExceptionSerializer CreateSerializer(ExceptionSerializerType serializerType)
        {
            switch (serializerType)
            {
                case ExceptionSerializerType.Xml:
                    return new XmlExceptionSerializer();
                case ExceptionSerializerType.Json:
                    return new JsonExceptionSerializer();
            }

            throw new NotSupportedException();
        }
    }
}