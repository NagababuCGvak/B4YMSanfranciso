using System;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Tar.Core.Mail
{
    public class ConsoleMailService : IMailService
    {
        public void SendMail(MailMessage mailMessage)
        {   
            var email = new StringBuilder();
            email.AppendLine(String.Format("To: {0}", string.Join(",", mailMessage.To.Select(i => i.Address).ToArray())));
            email.AppendLine(String.Format("Subject: {0}", mailMessage.Subject));
            email.AppendLine(String.Format("Body: {0}", mailMessage.Body));
            Console.WriteLine(email);
        }

        public void SendMail(string to, string subject, string body)
        {
            var email = new StringBuilder();
            email.AppendLine(String.Format("To: {0}", to));
            email.AppendLine(String.Format("Subject: {0}", subject));
            email.AppendLine(String.Format("Body: {0}", body));
            Console.WriteLine(email);
        }
    }
}
