using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;


namespace Better4You.UserManagment.Service.Implementation
{
    public class TextFileMailService : Tar.Core.Mail.IMailService
    {
        private readonly string _filePath;

        public TextFileMailService(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException("filePath");
            _filePath = filePath;
        }

        public void SendMail(MailMessage mailMessage)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _filePath);

            using (var file = new StreamWriter(path, true, Encoding.UTF8))
            {
                file.WriteLine("------------------------------------------------------------");
                file.WriteLine(DateTime.Now);
                file.WriteLine("To: {0}", mailMessage.To.First().Address);
                file.WriteLine("Subject: {0}", mailMessage.Subject);
                file.WriteLine("Body: {0}", mailMessage.Body);
                file.WriteLine();
                file.WriteLine();
                file.Flush();
            }
        }

        public void SendMail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            SendMail(mailMessage);
        }
    }
}