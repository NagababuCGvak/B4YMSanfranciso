using System.Net;
using System.Net.Mail;
using System.Text;

namespace Tar.Core.Mail
{
    public class StandartMailService : IMailService
    {
        private readonly string _mailAddress;
        private readonly SmtpClient _client;
        public string FromMailAddress { get; set; }

        public StandartMailService(string server, int port, bool enableSsl, string mailAddress, string password)
        {
            _mailAddress = mailAddress;
            _client = new SmtpClient(server, port)
                          {
                              Credentials = new NetworkCredential(mailAddress, password),
                              EnableSsl = enableSsl
                          };
        }

        public void SendMail(string to, string subject, string body)
        {
            var message = new MailMessage(_mailAddress, to)
                              {
                                  IsBodyHtml = true,
                                  Subject = subject,
                                  Body = body,
                                  SubjectEncoding = Encoding.UTF8,
                                  BodyEncoding = Encoding.UTF8
                              };
            _client.Send(message);
        }

        public void SendMail(MailMessage mailMessage)
        {
            var from = FromMailAddress;
            if (string.IsNullOrEmpty(from))
                from = _mailAddress;

            if (mailMessage.From ==null || string.IsNullOrEmpty(mailMessage.From.Address))
                mailMessage.From = new MailAddress(from);

            _client.Send(mailMessage);
        }
    }
}