using System.Runtime.Serialization;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    public class SchoolRouteRequest : Request
    {
        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public SchoolRouteView SchoolRoute { get; set; }
    }
}