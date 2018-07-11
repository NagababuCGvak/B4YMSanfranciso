using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Better4You.UserManagement.Config
{
    [DataContract]
    public enum FoodServiceType
    {
        [EnumMember]
        PREPACK=0,
        
        [EnumMember]
        FAMILYSTYLE=1,

        [EnumMember]
        PREPACK_NPB=2,

        [EnumMember]
        PREPACK_NP=3,

        [EnumMember]
        PREPACK_NCS=4,

        [EnumMember]
        PREPACK_NNUT=5,

        [EnumMember]
        FAMILYSTYLE_NP=6,

        [EnumMember]
        FAMILYSTYLE_PREW=7
    }
}
