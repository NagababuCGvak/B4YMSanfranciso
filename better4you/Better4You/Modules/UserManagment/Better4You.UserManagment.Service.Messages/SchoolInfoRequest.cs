using System.Runtime.Serialization;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class SchoolInfoRequest:Request
    {
        [DataMember]
        public SchoolView School { get; set; }
    }
}
