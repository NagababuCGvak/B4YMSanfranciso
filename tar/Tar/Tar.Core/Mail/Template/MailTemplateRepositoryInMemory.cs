using System.Collections.Generic;

namespace Tar.Core.Mail.Template
{
    public class MailTemplateRepositoryInMemory : IMailTemplateRepository
    {
        private readonly Dictionary<string, MailTemplate> _mailTemplates = new Dictionary<string, MailTemplate>();

        public MailTemplateRepositoryInMemory(IMailTemplateBuilder mailTemplateBuilder)
        {
            Save("test", mailTemplateBuilder
                             .Subject(
                                 "From MailTemplateRepositoryInMemory - Hello $source.FirstName $source.LastName")
                             .Body("Hello $source.FirstName $source.LastName,<br/><br/>How are you today?")
                             .Signature(
                                 "Signature:<br/> FirstName: $source.FirstName<br/>LastName:$source.LastName")
                             .AddDynamicSource("source",
                                               "Tar.Tests.Mail.Template.TestTemplateClass, Tar.Tests",
                                               "TestTemplateMethod")
                             .Build());
        }

        public MailTemplate GetMailTemplate(string key)
        {
            return _mailTemplates[key];
        }

        public void Save(string key, MailTemplate mailTemplate)
        {
            if(_mailTemplates.ContainsKey(key))
                _mailTemplates[key] = mailTemplate;
            else
                _mailTemplates.Add(key, mailTemplate);
        }
    }
}