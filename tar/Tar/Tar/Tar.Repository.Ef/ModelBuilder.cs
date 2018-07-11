using System.Data.Entity;
using Tar.Model;

namespace Tar.Repository.Ef
{
    public sealed class ModelBuilder : IModelBuilder
    {
        public void Build(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MailTemplateDefinition>().ToTable("MailTemplateDefinition");
            modelBuilder.Entity<MailTemplateDefinition>().HasKey(m => m.MailTemplateDefinitionId);
            modelBuilder.Entity<MailTemplateDefinition>().Property(m => m.Data).HasColumnType("XML");
        }
    }
}