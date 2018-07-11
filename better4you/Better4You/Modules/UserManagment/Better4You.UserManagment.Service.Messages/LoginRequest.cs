using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class LoginRequest : Request
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
}
