using System.Runtime.Serialization;

namespace Better4You.UserManagement.Config
{
    [DataContract]
    public enum AuthTypes
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Ldap = 1,
        [EnumMember]
        Database = 2,

    }
}
