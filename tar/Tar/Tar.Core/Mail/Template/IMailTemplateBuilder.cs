namespace Tar.Core.Mail.Template
{
    public interface IMailTemplateBuilder
    {
        IMailTemplateBuilder From(string mailAddress);
        IMailTemplateBuilder To(params string[] mailAddresses);
        IMailTemplateBuilder Cc(params string[] mailAddresses);
        IMailTemplateBuilder Bcc(params string[] mailAddresses);
        IMailTemplateBuilder Subject(string subject);
        IMailTemplateBuilder Body(string body);
        IMailTemplateBuilder Signature(string signature);
        MailTemplate Build();
        IMailTemplateBuilder AddStaticSource(string key, object value);
        IMailTemplateBuilder AddDynamicSource(string key, string typeName, string methodName);
    }
}