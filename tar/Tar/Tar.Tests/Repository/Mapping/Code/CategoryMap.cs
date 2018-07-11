using Tar.Tests.Model;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Tar.Tests.Repository.Mapping.Code
{
    public class CategoryMap:ClassMapping<Category>
    {
        public CategoryMap()
        {
            Table("Category");
            Lazy(true);
            Id(x => x.Id,
               map =>
                   {
                       map.Column("CategoryId");                       
                       map.Generator(Generators.Identity);
                   }
                );
            Property(x => x.Name,
                     map => map.Column(c =>
                                           {
                                               c.Name("CategoryName");
                                               c.SqlType("nvarchar(200)");
                                           }
                                )
                );
        }
    }
}
