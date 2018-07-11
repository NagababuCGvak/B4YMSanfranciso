using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class UserMap:ClassMapping<User>
    {
        public UserMap()
        {
            Table("UM_USER");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("USERID");
                   map.Generator(Generators.Native);
               });

            Property(x => x.UserName,
                     map => map.Column(c =>
                                           {
                                               c.Name("USERNAME");
                                               c.SqlType("nvarchar(256)");
                                               c.NotNullable(true);
                                               c.Unique(true);
                                           }
                                )
                );

            Property(x => x.FirstName,
                     map => map.Column(c =>
                                           {
                                               c.Name("FIRSTNAME");
                                               c.SqlType("nvarchar(64)");
                                               c.NotNullable(true);
                                           }
                                )
                );

            Property(x => x.LastName,
                     map => map.Column(c =>
                     {
                         c.Name("LASTNAME");
                         c.SqlType("nvarchar(64)");
                         c.NotNullable(true);
                     }
                                )
                );

            Property(x => x.Password,
                     map => map.Column(c =>
                                           {
                                               c.Name("PASSWORD");
                                               c.SqlType("nvarchar(256)");
                                               c.NotNullable(true);
                                           }
                                )
                );
            Property(x => x.PasswordSalt,
                     map => map.Column(c =>
                     {
                         c.Name("PASSWORDSALT");
                         c.SqlType("nvarchar(256)");
                         c.NotNullable(true);
                     }
                                )
                );
            ManyToOne(p => p.PasswordFormatType,
                      map =>
                          {
                              map.Column("PASSWORDFORMATTYPEID");
                              map.Lazy(LazyRelation.NoLazy);
                              map.Cascade(Cascade.None);
                              map.NotNullable(true);
                          }
                );
            ManyToOne(p => p.UserType,
                      map =>
                      {
                          map.Column("USERTYPEID");
                          map.Lazy(LazyRelation.NoLazy);
                          map.Cascade(Cascade.None);
                          map.NotNullable(true);
                      }
                );
            ManyToOne(p => p.AlgorithmType,
                      map =>
                      {
                          map.Column("ALGORITHMTYPEID");
                          map.Lazy(LazyRelation.NoLazy);
                          map.Cascade(Cascade.None);
                          map.NotNullable(true);
                      }
                );


            OneToOne(o => o.UserLoginInfo,
                map =>
                {
                    map.PropertyReference(typeof(UserLoginInfo).GetProperty("User"));
                    map.Cascade(Cascade.All);
                    //map.Constrained(true);
                    //map.Lazy(LazyRelation.NoLazy);
                });
            
            
            Bag(p => p.Applications,
                map =>
                    {
                        map.Key(k => k.Column("USERID"));
                        map.Table("UM_APPLICATION_USER");
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("APPLICATIONID");
                                            l.Class(typeof (Application));
                                        })
                );
            
            Bag(p => p.Schools,
                map =>
                    {
                        map.Key(k => k.Column("USERID"));
                        map.Cascade(Cascade.All);
                        map.Table("UM_USER_SCHOOL");
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("SCHOOLID");
                                            l.Class(typeof (School));
                                        })
                );
            
            Bag(p => p.Roles,
                map =>
                    {
                        map.Key(k => k.Column("USERID"));
                        map.Cascade(Cascade.All);
                        map.Table("UM_USER_ROLE");
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("ROLEID");
                                            l.Class(typeof (Role));
                                        })
                );
            
            Bag(p => p.Communications,
                map =>
                    {
                        map.Key(k => k.Column("USERID"));
                        map.Cascade(Cascade.All);
                        map.Table("UM_USER_COMMUNICATION");
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("COMMUNICATIONID");
                                            l.Class(typeof (Communication));
                                        })
                );
             
        }
    }
}
