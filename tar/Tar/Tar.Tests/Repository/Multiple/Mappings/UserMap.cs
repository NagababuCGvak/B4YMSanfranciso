using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Tar.Tests.Repository.Multiple.Model;

namespace Tar.Tests.Repository.Multiple.Mappings
{
    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Lazy(true);
            Id(x => x.Id, mapper => mapper.Generator(Generators.Native));
            Property(x => x.UserName);
            Property(x => x.Password);
        }
    }
}