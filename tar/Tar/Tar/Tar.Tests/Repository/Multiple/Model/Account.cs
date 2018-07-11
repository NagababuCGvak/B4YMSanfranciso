using Tar.Tests.Repository.Multiple.Repository;

namespace Tar.Tests.Repository.Multiple.Model
{
    public class Account : ITenantEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
    }
}