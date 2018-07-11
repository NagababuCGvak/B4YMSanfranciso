using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Tar.Core.Mail.Template;
using Tar.Model;

namespace Tar.Repository.General
{
    /// <summary>
    /// IF OBJECT_ID('[dbo].[MailTemplateDefinition]')>0
    /// 	Drop Table [dbo].[MailTemplateDefinition]
    /// go
    /// Create Table [dbo].[MailTemplateDefinition]
    /// (
    /// 	[Id] int identity,
    /// 	[Key] varchar(100) not null ,
    /// 	[Data] nvarchar(max) not null,
    /// 	CONSTRAINT PK_MailTemplateDefinition_Id PRIMARY KEY([Id]),
    /// 	CONSTRAINT UQ_MailTemplateDefinition_Key UNIQUE ([Key])
    /// )
    /// go
    /// </summary>
    public class MailTemplateRepository : IMailTemplateRepository
    {
        private readonly IGeneralRepository _repository;
        private readonly XmlSerializer _serializer;

        public MailTemplateRepository(IGeneralRepository repository)
        {
            _repository = repository;
            _serializer = new XmlSerializer(typeof (MailTemplate));
        }

        public MailTemplate GetMailTemplate(string key)
        {
            var entity = _repository.Query<MailTemplateDefinition>().First(m => m.Key == key);
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(entity.Data)))
            {
                var template = (MailTemplate)_serializer.Deserialize(stream);
                return template;
            }
        }

        public void Save(string key, MailTemplate mailTemplate)
        {
            var entity = _repository.Query<MailTemplateDefinition>().FirstOrDefault(m => m.Key == key);
            using (var stringWriter = new StringWriter())
            {
                _serializer.Serialize(stringWriter, mailTemplate);

                if (entity == null)
                {
                    entity = new MailTemplateDefinition { Key = key, Data = stringWriter.ToString() };
                    _repository.Create(entity);
                }
                else
                {
                    entity.Data = stringWriter.ToString();
                    _repository.Update(entity);
                }
            }
        }
    }
}
