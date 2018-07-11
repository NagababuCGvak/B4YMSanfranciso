using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class GetApplicationUsersRequest : Request
    {
        [Required]
        [DataMember]
        public long ApplicationId { get; set; }

        [DataMember]
        [Required]
        public UserViewModelFilter Filter { get; set; }
    }
}
