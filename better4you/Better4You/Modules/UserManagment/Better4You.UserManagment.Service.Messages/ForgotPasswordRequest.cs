using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class ForgotPasswordRequest : Request
    {
        [DataMember]
        public string UserName { get; set; }
    }
}
