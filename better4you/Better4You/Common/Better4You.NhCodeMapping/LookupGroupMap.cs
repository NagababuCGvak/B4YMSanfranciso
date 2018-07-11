using Better4You.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.NhCodeMapping
{
    public class LookupGroupMap : ClassMapping<LookupGroup>
    {
        public LookupGroupMap()
        {
            Table("GNRL_LOOKUPGROUP");
            Lazy(true);
            Id(x => x.Id,
               map =>
                   {
                       map.Column("LOOKUPGROUPID");
                       map.Generator(Generators.Assigned);
                   });

            Property(x => x.Name,
                     map => map.Column(c =>
                                           {
                                               c.Name("NAME");
                                               c.SqlType("nvarchar(256)");
                                               c.NotNullable(false);
                                           }
                                )
                );
            Bag(p => p.LookupItems,
                map =>
                    {
                        map.Key(k =>
                                    {
                                        k.Column("LOOKUPGROUPID");
                                        k.OnDelete(OnDeleteAction.NoAction);
                                    });
                        map.Cascade(Cascade.All);
                        map.Inverse(true);
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.OneToMany()
                );
        }

    }
}
