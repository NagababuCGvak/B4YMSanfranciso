using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class ApplicationMap:ClassMapping<Application>
    {
        public ApplicationMap()
        {
            Table("UM_APPLICATION");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("APPLICATIONID");
                   map.Generator(Generators.Native);
               });

            Property(x => x.Name,
                     map => map.Column(c =>
                                           {
                                               c.Name("NAME");
                                               c.SqlType("nvarchar(512)");
                                               c.NotNullable(true);
                                           }
                                )
                );


            //Bag(p => p.Users,
            //    map =>
            //    {
            //        map.Key(k => k.Column("USERID"));
            //        map.Cascade(Cascade.All);
            //        map.Table("UM_APPLICATION_USER");
            //        map.Lazy(CollectionLazy.Lazy);
            //    },
            //    ce => ce.ManyToMany(l => { l.Column("APPLICATIONID"); l.Class(typeof(User)); })
            //    );
            Bag(p => p.Users,
                map =>
                    {
                        map.Key(k => k.Column("APPLICATIONID"));
                        map.Cascade(Cascade.All);
                        map.Table("UM_APPLICATION_USER");
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
