using System;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    public class GetUserRolesRequest : Request
    {
        public long UserId { get; set; }

        public override void Validate()
        {
            base.Validate();
            if (UserId == 0) throw new Exception("Please select user!");
        }
    }
}