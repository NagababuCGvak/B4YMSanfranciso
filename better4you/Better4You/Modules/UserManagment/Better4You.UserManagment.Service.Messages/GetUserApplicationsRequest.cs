using System;
using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class GetUserApplicationsRequest : Request
    {
        [DataMember]
        public long UserId { get; set; }
        public override void Validate()
        {
            if(UserId==0) throw new Exception("UserId is Zero!");
        }
    }
}
