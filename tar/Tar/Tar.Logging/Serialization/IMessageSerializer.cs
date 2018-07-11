using System;

namespace Tar.Logging.Serialization
{
    public interface IMessageSerializer
    {
        event EventHandler<MessageSerializerEventArgs> Serialized;
        string Serialize(object message);
    }
}