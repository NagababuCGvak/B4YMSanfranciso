using System.Runtime.Serialization;

namespace Better4You.UserManagement.Config
{
    [DataContract]
    public enum UserTypes
    {
        [EnumMember]
        None = 0,
        
        [EnumMember]
        Standart = 1,

        [EnumMember]
        Integration = 2,
        
        [EnumMember]
        EndUser = 3,

    }
}
