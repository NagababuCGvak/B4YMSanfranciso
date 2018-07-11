using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class GetUserRequest : Request
    {
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
    }
}