using System;

namespace Tar.Core.LoggingOld2.Serialization
{
    public interface IMessageSerializer
    {
        event EventHandler<MessageSerializerEventArgs> Serialized;
        string Serialize(object message);
    }
}