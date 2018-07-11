using System;
using Better4You.Core.Repositories;

namespace Better4You.UserManagment.EntityModel
{
    public class UserLoginInfo : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsOnline { get; set; }
        public virtual bool IsApprove { get; set; }
        public virtual bool IsLocked { get; set; }
        public virtual DateTime? LastLoginDate { get; set; }
        public virtual int? PasswordAttemptCount { get; set; }
        public virtual DateTime? LastPasswordAttemptDate { get; set; }
        public virtual string ActivationCode { get; set; }
        public virtual DateTime? ExpireActivationDate { get; set; }
        public virtual string Description { get; set; }
        public virtual User User { get; set; }

    }
}