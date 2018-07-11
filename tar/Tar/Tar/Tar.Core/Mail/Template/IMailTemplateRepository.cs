namespace Tar.Core.Mail.Template
{
    public interface IMailTemplateRepository
    {
        MailTemplate GetMailTemplate(string key);
        void Save(string key, MailTemplate mailTemplate);
    }
}
