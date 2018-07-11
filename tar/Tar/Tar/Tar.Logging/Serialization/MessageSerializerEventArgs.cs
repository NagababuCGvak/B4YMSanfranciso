namespace Tar.Logging.Serialization
{
    public class MessageSerializerEventArgs : System.EventArgs
    {
        public object Message { get; set; }
        public string SerializedMessage { get; set; }

        public MessageSerializerEventArgs(object message, string serializedMessage)
        {
            Message = message;
            SerializedMessage = serializedMessage;
        }
    }
}