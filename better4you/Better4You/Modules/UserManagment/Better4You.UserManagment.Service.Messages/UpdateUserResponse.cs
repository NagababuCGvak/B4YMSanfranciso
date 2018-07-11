using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class UpdateUserResponse : Response
    {
        [DataMember]
        public long UserId;
    }
}