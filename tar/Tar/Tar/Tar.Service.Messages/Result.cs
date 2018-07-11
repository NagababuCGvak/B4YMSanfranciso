using System.Runtime.Serialization;

namespace Tar.Service.Messages
{
    [DataContract]
    public enum Result
    {
        [EnumMember]
        Success = 1,

        [EnumMember]
        Failed = 0
    }
}