namespace Tar.Model
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
    public class MailTemplateDefinition:IEntity
    {
        public virtual int MailTemplateDefinitionId { get; set; }
        public virtual string Key { get; set; }
        public virtual string Data { get; set; }
    }
}
