using System.Runtime.Serialization;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class UpdateUserRequest : Request
    {
        [DataMember]
        public UserViewModel User { get; set; }
        [DataMember]
        public long ApplicationId { get; set; }

        public UpdateUserRequest()
        {
            User = new UserViewModel();
        }
    }
}