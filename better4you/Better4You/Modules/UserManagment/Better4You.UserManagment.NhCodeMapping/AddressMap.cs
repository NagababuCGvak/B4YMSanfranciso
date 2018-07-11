using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class AddressMap:ClassMapping<Address>
    {
        public AddressMap()
        {
            Table("UM_ADDRESS");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("ADDRESSID");
                   map.Generator(Generators.Native);
               });

            Property(x => x.Address1,
                     map => map.Column(c =>
                                           {
                                               c.Name("ADDRESS1");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Address2,
                     map => map.Column(c =>
                                           {
                                               c.Name("ADDRESS2");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(false);
                                           }
                                )
                );
            
            Property(x => x.PostalCode,
                     map => map.Column(c =>
                                           {
                                               c.Name("POSTALCODE");
                                               c.SqlType("nvarchar(16)");
                                               c.NotNullable(false);
                                           }
                                )
                );
            
            Property(x => x.District,
                     map => map.Column(c =>
                                           {
                                               c.Name("DISTRICT");
                                               c.SqlType("nvarchar(64)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            ManyToOne(p => p.AddressType,
                      map =>
                          {
                              map.Column("ADDRESSTYPEID");
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

            ManyToOne(p => p.Country,
                      map =>
                          {
                              map.Column("COUNTRYID");
                              map.Lazy(LazyRelation.NoLazy);
                              map.NotNullable(true);
                          }
                );
            ManyToOne(p => p.FirstAdminDivision,
                      map =>
                          {
                              map.Column("FIRSTADMINDIVISIONID");
                              map.Lazy(LazyRelation.NoLazy);
                              map.NotNullable(true);
                          }
                );
            ManyToOne(p => p.City,
                      map =>
                          {
                              map.Column("CITYID");
                              map.Lazy(LazyRelation.NoLazy);
                              map.NotNullable(true);
                          }
                );
            
            Bag(p => p.Schools,
                map =>
                    {
                        map.Key(k => k.Column("ADDRESSID"));
                        map.Table("UM_SCHOOL_ADDRESS");
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
