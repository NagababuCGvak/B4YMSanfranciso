using System;

namespace Better4You.UserManagment.Business
{
    public interface IUserNotifications
    {
        void ResetPassword(long userId, string activationCode, DateTime expireActivationDate);
    }
}
