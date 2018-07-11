using System.Net.Mail;

namespace Tar.Core.Mail
{
    public interface IMailService
    {
        void SendMail(MailMessage mailMessage);
        void SendMail(string to, string subject, string body);
    }
}
