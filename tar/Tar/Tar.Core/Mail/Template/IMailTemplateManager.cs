using System;
using System.Net.Mail;

namespace Tar.Core.Mail.Template
{
    public interface IMailTemplateManager
    {
        MailTemplate CreateMailTemplate(Func<IMailTemplateBuilder, IMailTemplateBuilder> func);
        MailTemplate GetMailTemplate(string key);

        void SendMail(SendMailParameter parameter);
        MailMessage GenerateMessage(SendMailParameter parameter);
    }
}