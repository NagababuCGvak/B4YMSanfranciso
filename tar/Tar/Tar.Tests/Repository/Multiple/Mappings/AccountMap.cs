using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Tar.Tests.Repository.Multiple.Model;

namespace Tar.Tests.Repository.Multiple.Mappings
{
    public class AccountMap : ClassMapping<Account>
    {
        public AccountMap()
        {
            Lazy(true);
            Id(x => x.Id, mapper => mapper.Generator(Generators.Native));
            Property(x => x.Code);
            Property(x => x.Name);
        }
    }
}