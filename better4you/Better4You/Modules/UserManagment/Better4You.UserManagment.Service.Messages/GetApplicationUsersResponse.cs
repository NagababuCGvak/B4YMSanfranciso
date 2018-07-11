using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class GetApplicationUsersResponse : Response
    {
        [DataMember]
        public IEnumerable<UserViewModel> Users { get; set; }

        public GetApplicationUsersResponse()
        {
            Users = Enumerable.Empty<UserViewModel>();
        }
    }
}