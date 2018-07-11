using System;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    public class AddRoleToUserRequest : Request
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public override void Validate()
        {
            base.Validate();
            if (UserId == 0) throw new Exception("Please select user.");
            if (RoleId == 0) throw new Exception("Please select role.");
        }
    }
}
