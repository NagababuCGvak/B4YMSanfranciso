using System;
using System.Runtime.CompilerServices;
using System.Text;
using Better4You.Core.Repositories;
using Better4You.UserManagment.EntityModel;
using Tar.Core.Configuration;
using Tar.Core.Mail;
using Tar.Repository.General;

namespace Better4You.UserManagment.Business.Implementation
{
    public class UserNotifications : IUserNotifications
    {
        public IConfigRepository Repository { get; set; }
        public IMailService MailService { get; set; }
        public ISettings Settings { get; set; }

        public void ResetPassword(long userId, string activationCode, DateTime expireActivationDate)
        {
            var user = Repository.GetById<User>(userId);
            var mailAddress = user.UserName;
            var subject = string.Format("Change Password - {0},{1}", user.LastName, user.FirstName);
            var body = new StringBuilder()
                .AppendFormat("{0}/Account/ChangePassword?ActivationCode={1}", Settings.GetSetting<string>("HostServer"), activationCode)
                .AppendLine("<br/>")
                .AppendFormat("ValidDate:{0}", expireActivationDate);
           
            MailService.SendMail(mailAddress, subject, body.ToString());
            subject = String.Format("User:{0} {1} clicked on forgot password link, and the email below sent to: {2}",
                user.LastName, user.FirstName, user.UserName);
           MailService.SendMail("droberts@better4youmeals.com", subject, body.ToString());
           // MailService.SendMail("volkan.uzun@gmail.com", subject, body.ToString());
        }
    }
}
