using Better4You.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.NhCodeMapping
{
    public class FirstAdminDivisionMap:ClassMapping<FirstAdminDivision>
    {
        public FirstAdminDivisionMap()
        {
            Table("GNRL_FIRSTADMINDIVISION");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("FIRSTADMINDIVISIONID");
                   map.Generator(Generators.Native);
               });
            
            Property(x => x.Abbreviation,
                     map => map.Column(c =>
                                           {
                                               c.Name("ABBREVIATION");
                                               c.SqlType("nvarchar(16)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Name,
                     map => map.Column(c =>
                                           {
                                               c.Name("NAME");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Bag(p => p.Cities,
                map =>
                    {
                        map.Key(k =>
                                    {
                                        k.Column("COUNTRYID");
                                        k.OnDelete(OnDeleteAction.NoAction);
                                    });
                        map.Cascade(Cascade.All);
                        map.Inverse(true);
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.OneToMany()
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
