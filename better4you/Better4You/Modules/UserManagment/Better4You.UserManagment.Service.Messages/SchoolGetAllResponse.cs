using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class SchoolGetAllResponse : PageableResponse
    {
        [DataMember]
        public List<SchoolListItemView> Schools { get; set; }

        public SchoolGetAllResponse()
        {
            Schools=new List<SchoolListItemView>();
        }
    }
}