using System.Runtime.Serialization;

namespace Tar.Service.Messages
{
    [DataContract]
    public abstract class Response
    {
        [DataMember]
        public Result Result { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}