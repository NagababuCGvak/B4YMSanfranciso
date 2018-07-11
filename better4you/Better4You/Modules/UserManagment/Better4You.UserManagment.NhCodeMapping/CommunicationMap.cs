using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class CommunicationMap:ClassMapping<Communication>
    {
        public CommunicationMap()
        {
            Table("UM_COMMUNICATION");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("COMMUNICATIONID");
                   map.Generator(Generators.Native);
               });

            Property(x => x.Description,
                     map => map.Column(c =>
                                           {
                                               c.Name("DESCRIPTION");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Phone,
                     map => map.Column(c =>
                                           {
                                               c.Name("PHONE");
                                               c.SqlType("nvarchar(16)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Extension,
                     map => map.Column(c =>
                                           {
                                               c.Name("EXTENSION");
                                               c.SqlType("nvarchar(8)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Email,
                     map => map.Column(c =>
                                           {
                                               c.Name("EMAIL");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.IsPrimary,
                     map => map.Column(c =>
                                           {
                                               c.Name("ISPRIMARY");
                                               c.SqlType("bit");
                                               c.NotNullable(false);
                                               c.Default(0);
                                           }
                                )
                );
            ManyToOne(p => p.CommunicationType,
                      map =>
                          {
                              map.Column("COMMUNICATIONTYPEID");
                              map.Lazy(LazyRelation.NoLazy);
                              map.Cascade(Cascade.None);
                              map.NotNullable(true);
                          }
                );
            ManyToOne(p => p.RecordStatus,
                      map =>
                      {
                          map.Column("RECORDSTATUSID");
                          map.Lazy(LazyRelation.NoLazy);
                          map.Cascade(Cascade.None);
                          map.NotNullable(true);
                      }
                );


            
            Bag(p => p.Users,
                map =>
                    {
                        map.Key(k => k.Column("COMMUNICATIONID"));
                        map.Table("UM_USER_COMMUNICATION");
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("USERID");
                                            l.Class(typeof (User));
                                        })
                );
            
            Bag(p => p.Schools,
                map =>
                    {
                        map.Key(k => k.Column("COMMUNICATIONID"));
                        map.Table("UM_SCHOOL_COMMUNICATION");
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("SCHOOLID");
                                            l.Class(typeof (School));
                                        })
                );
            
        }
    }
}
