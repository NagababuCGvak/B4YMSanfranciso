using System;
using System.Runtime.Serialization;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class CreateUserRequest : Request
    {
        [DataMember]
        public UserViewModel User { get; set; }
        [DataMember]
        public long ApplicationId { get; set; }

        public CreateUserRequest()
        {
            User = new UserViewModel();
        }
        public override void Validate()
        {
            base.Validate();
            if (User.UserTypeId == 0) throw new Exception("Select user type!");
        }
    }
}