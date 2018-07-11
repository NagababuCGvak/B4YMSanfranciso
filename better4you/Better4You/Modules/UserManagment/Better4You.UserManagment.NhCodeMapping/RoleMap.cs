using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class RoleMap:ClassMapping<Role>
    {
        public RoleMap()
        {
            Table("UM_ROLE");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("ROLEID");
                   map.Generator(Generators.Native);
               });

            Property(x => x.Code,
                     map => map.Column(c =>
                                           {
                                               c.Name("CODE");
                                               c.SqlType("nvarchar(64)");
                                               c.NotNullable(true);
                                           }
                                )
                );

            Property(x => x.Name,
                     map => map.Column(c =>
                                           {
                                               c.Name("NAME");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(true);
                                           }
                                )
                );
            
            Bag(p => p.Users,
                map =>
                    {
                        map.Key(k => k.Column("ROLEID"));
                        map.Table("UM_USER_ROLE");
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("USERID");
                                            l.Class(typeof (User));
                                        })
                );
            
        }
    }
}
