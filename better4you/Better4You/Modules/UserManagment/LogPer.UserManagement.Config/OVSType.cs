using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Better4You.UserManagement.Config
{
    [DataContract]
    public enum OVSType
    {
        [EnumMember]
        ServeOnly=0,

        [EnumMember]
        OffervsServe=1
    }
}
