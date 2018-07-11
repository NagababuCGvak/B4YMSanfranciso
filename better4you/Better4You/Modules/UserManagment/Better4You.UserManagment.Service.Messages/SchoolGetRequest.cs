using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class SchoolGetRequest:Request
    {
        [DataMember]
        public long Id { get; set; }
    }
}