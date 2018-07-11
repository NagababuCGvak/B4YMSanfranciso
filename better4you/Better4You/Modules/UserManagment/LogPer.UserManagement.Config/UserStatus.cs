using System.Runtime.Serialization;

namespace Better4You.UserManagement.Config
{
    [DataContract]
    public enum UserStatus
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Active = 1,
        [EnumMember]
        Deleted = 2,

    }
}
