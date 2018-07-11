using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class SchoolMap : ClassMapping<School>
    {
        public SchoolMap()
        {
            Table("UM_SCHOOL");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("SCHOOLID");
                   map.Generator(Generators.Native);
               });

            Property(x => x.Name,
                     map => map.Column(c =>
                                           {
                                               c.Name("NAME");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(true);
                                           }
                                )
                );

            Property(x => x.Code,
                     map => map.Column(c =>
                                           {
                                               c.Name("CODE");
                                               c.SqlType("nvarchar(32)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Address1,
                map => map.Column(c =>
                {
                    c.Name("ADDRESS1");
                    c.SqlType("nvarchar(128)");
                    c.NotNullable(false);
                }));

            Property(x => x.Address2,
                map => map.Column(c =>
                {
                    c.Name("ADDRESS2");
                    c.SqlType("nvarchar(128)");
                    c.NotNullable(false);
                }));

            Property(x => x.ZipCode,
                map => map.Column(c =>
                {
                    c.Name("ZIPCODE");
                    c.SqlType("nvarchar(16)");
                    c.NotNullable(false);
                }));


            Property(x => x.FirstAdminDivision,
                map => map.Column(c =>
                {
                    c.Name("FIRSTADMINDIVISION");
                    c.SqlType("nvarchar(128)");
                    c.NotNullable(false);
                }));

            Property(x => x.City,
                map => map.Column(c =>
                {
                    c.Name("CITY");
                    c.SqlType("nvarchar(128)");
                    c.NotNullable(false);
                }));

            Property(x => x.BusinessPhone,
                map => map.Column(c =>
                {
                    c.Name("BUSINESSPHONE");
                    c.SqlType("nvarchar(16)");
                    c.NotNullable(false);
                }));

            Property(x => x.CellPhone,
                map => map.Column(c =>
                {
                    c.Name("CELLPHONE");
                    c.SqlType("nvarchar(16)");
                    c.NotNullable(false);
                }));

            Property(x => x.Fax,
                map => map.Column(c =>
                {
                    c.Name("FAX");
                    c.SqlType("nvarchar(16)");
                    c.NotNullable(false);
                }));

            Property(x => x.Email,
                map => map.Column(c =>
                {
                    c.Name("EMAIL");
                    c.SqlType("nvarchar(128)");
                    c.NotNullable(false);
                }));

            Property(x => x.Notes,
                map => map.Column(c =>
                {
                    c.Name("NOTES");
                    c.SqlType("nvarchar(512)");
                    c.NotNullable(false);
                }));

            Property(p => p.FirstAdminDivisionId,
                map =>
                    map.Column(c =>
                    {
                        c.Name("FIRSTADMINDIVISIONID");
                        c.SqlType("int");
                        c.NotNullable(true);
                    })
                );

            Property(p => p.CityId,
                map =>
                    map.Column(c =>
                    {
                        c.Name("CITYID");
                        c.SqlType("int");
                        c.NotNullable(true);
                    })
                );

            Property(p => p.SchoolType,
                map =>
                    map.Column(c =>
                    {
                        c.Name("SCHOOLTYPE");
                        c.SqlType("int");
                        c.NotNullable(true);
                    })
                );

            Property(p => p.FoodServiceType,
                map =>
                    map.Column(c =>
                    {
                        c.Name("FOODSERVICETYPE");
                        c.SqlType("int");
                        c.NotNullable(true);
                    })
                );

            Property(p => p.OVSType,
                map =>
                    map.Column(c =>
                    {
                        c.Name("OVSType");
                        c.SqlType("int");
                        c.NotNullable(true);
                    })
                );

            Property(p => p.RecordStatus,
                map =>
                    map.Column(c =>
                    {
                        c.Name("STATUS");
                        c.SqlType("int");
                        c.NotNullable(true);
                    })
                );


            Bag(p => p.Users,
                map =>
                {
                    map.Key(k => k.Column("SCHOOLID"));
                    map.Table("UM_USER_SCHOOL");
                    map.Lazy(CollectionLazy.Lazy);
                },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("USERID");
                                            l.Class(typeof(User));
                                        })
                );


            Bag(p => p.SchoolAnnualAgreements,
                map =>
                {
                    map.Key(k =>
                                {
                                    k.Column("SCHOOLID");
                                    k.OnDelete(OnDeleteAction.Cascade);
                                });
                    map.Cascade(Cascade.All);
                    map.Inverse(true);
                    map.Lazy(CollectionLazy.Lazy);
                },
                ce => ce.OneToMany());



            Bag(p => p.SchoolRoutes,
            map =>
            {
                map.Key(k =>
                {
                    k.Column("SCHOOLID");
                    k.OnDelete(OnDeleteAction.Cascade);
                });
                map.Cascade(Cascade.All);
                map.Inverse(true);
                map.Lazy(CollectionLazy.Lazy);
            },
            ce => ce.OneToMany());


        }
    }
}
