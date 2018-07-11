using System.Net.Mail;

namespace Tar.Core.Mail
{
    public class SmtpMailService : IMailService
    {
        public void SendMail(MailMessage mailMessage)
        {
            var smtp = new SmtpClient();
            smtp.Send(mailMessage);
        }

        public void SendMail(string to, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            
            SendMail(message);
        }
    }
}
