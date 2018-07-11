using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class SchoolUserRequest:Request
    {   
        [DataMember]
        public long SchoolId { get; set; }
        
        [DataMember]
        public long UserId { get; set; }
    }
}