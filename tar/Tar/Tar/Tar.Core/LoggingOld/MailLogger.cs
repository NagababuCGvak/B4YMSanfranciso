using System;
using System.Net.Mail;
using System.Text;
using Tar.Core.Mail;

namespace Tar.Core.LoggingOld
{
    public class MailLogger : Logger
    {
        private readonly IMailService _mailService;
        private readonly string _mailAddress;
        private readonly string _to;

        public MailLogger(string mailAddress, string password, string to)
        {
            _mailService = new GmailService(mailAddress, password);
            _to = to;
            _mailAddress = mailAddress;
        }

        protected override void SendToLog(string source, LogType logType, string message)
        {
            var mailMessage = new MailMessage(new MailAddress(_mailAddress), new MailAddress(_to))
                                  {
                                      //SubjectEncoding = Encoding.GetEncoding("utf-8"),
                                      //BodyEncoding = Encoding.GetEncoding("tr-TR"),
                                      Subject = string.Format("{0}:{1} - {2}", source, logType, DateTime.Now),
                                      IsBodyHtml = true
                                  };

            var messageBody = new StringBuilder();
            messageBody.Append("<table>");
            messageBody.AppendFormat("<tr><td>Log Date: </td><td>{0}</td></tr>", DateTime.Now);
            messageBody.AppendFormat("<tr><td>Log Category: </td><td>{0}</td></tr>", source);
            messageBody.AppendFormat("<tr><td>Log Type: </td><td>{0}</td></tr>", logType);
            messageBody.AppendFormat("<tr><td>Log Message: </td><td>{0}</td></tr>", message);
            messageBody.Append("</table>");

            mailMessage.Body = messageBody.ToString();

            _mailService.SendMail(mailMessage);

        }
    }
}