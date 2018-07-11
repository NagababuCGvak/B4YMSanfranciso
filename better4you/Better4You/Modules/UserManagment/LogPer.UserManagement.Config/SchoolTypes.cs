using System.Runtime.Serialization;

namespace Better4You.UserManagement.Config
{
    [DataContract]
    public enum SchoolTypes
    {
        [EnumMember]
        None = 0,
        
        /*
        [EnumMember]
        Elem = 1,

        [EnumMember]
        ElemMs = 2,
        
        [EnumMember]
        ElemHs = 3,

        [EnumMember]
        Ms = 4,

        [EnumMember]
        Hs = 5,

        [EnumMember]
        MsHs = 6,
        */


        [EnumMember]
        Hs = 5,

        [EnumMember]
        K8 = 7

    
    }
}
