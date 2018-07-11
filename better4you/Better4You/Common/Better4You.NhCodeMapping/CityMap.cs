using Better4You.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.NhCodeMapping
{
    public class CityMap : ClassMapping<City>
    {
        public CityMap()
        {
            Table("GNRL_CITY");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("CITYID");
                   map.Generator(Generators.Native);
               });


            Property(x => x.Name,
                     map => map.Column(c =>
                     {
                         c.Name("NAME");
                         c.SqlType("nvarchar(128)");
                         c.NotNullable(false);
                     }
                                )
                );

            ManyToOne(p => p.FirstAdminDivision,
                      map =>
                          {
                              map.Column("FIRSTADMINDIVISIONID");
                              map.Lazy(LazyRelation.NoLazy);
                              map.NotNullable(true);
                          }
                );

            ManyToOne(p => p.Country,
                      map =>
                          {
                              map.Column("COUNTRYID");
                              map.Lazy(LazyRelation.NoLazy);
                              map.NotNullable(true);
                          }
                );
        }
    }
}
