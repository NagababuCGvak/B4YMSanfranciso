using Better4You.Meal.EntityModel;
using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class MenuMap:ClassMapping<Menu>
    {
        public MenuMap()
        {
            Table("NTR_MENU");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("MENUID");
                   map.Generator(Generators.Identity);
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
            Property(x => x.IsExclusive,
                     map => map.Column(c =>
                                           {
                                               c.Name("ISEXCLUSIVE");
                                               c.SqlType("bit");
                                               c.Default(0);
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.AdditionalFruit,
                map => map.Column(c =>
                {
                    c.Name("ADDITIONALFRUIT");
                    c.SqlType("bit");
                    c.Default(0);
                    c.NotNullable(false);
                }
                    )
                );

            Property(x => x.AdditionalVeg,
                map => map.Column(c =>
                {
                    c.Name("ADDITIONALVEG");
                    c.SqlType("bit");
                    c.Default(0);
                    c.NotNullable(false);
                }
                    )
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

            Property(p => p.MenuType,
                map =>
                    map.Column(c =>
                               {
                                   c.Name("MENUTYPE");
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

            Bag(p => p.Foods,
                map =>
                    {
                        map.Key(k => k.Column("MENUID"));
                        map.Cascade(Cascade.All);
                        map.Table("NTR_MENU_FOOD");
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("FOODID");
                                            l.Class(typeof (Food));
                                        }));

            Bag(p => p.Schools,
                map =>
                    {
                        map.Key(k => k.Column("MENUID"));
                        map.Cascade(Cascade.All);
                        map.Table("NTR_MENU_SCHOOL");
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.ManyToMany(l =>
                                        {
                                            l.Column("SCHOOLID");
                                            l.Class(typeof (School));
                                        })
                );

            Property(p => p.CreatedAt,
                map => map.Column(c =>
                                  {
                                      c.Name("CREATEDDATE");
                                      c.SqlType("datetime");
                                      c.NotNullable(true);
                                  }
                    ));

            Property(p => p.CreatedBy,
                map => map.Column(c =>
                                  {
                                      c.Name("CREATEDBY");
                                      c.SqlType("nvarchar(256)");
                                      c.NotNullable(true);                       
                                  }
                    ));

            Property(p => p.CreatedByFullName,
                map => map.Column(c =>
                                  {
                                      c.Name("CREATEDBYFULLNAME");
                                      c.SqlType("nvarchar(256)");
                                      c.NotNullable(true);
                                  }
                    ));

            Property(p => p.ModifiedAt,
                map => map.Column(c =>
                                  {
                                      c.Name("MODIFIEDDATE");
                                      c.SqlType("datetime");
                                      c.NotNullable(true);
                                  }
                    ));

            Property(p => p.ModifiedBy,
                map => map.Column(c =>
                                  {
                                      c.Name("MODIFIEDBY");
                                      c.SqlType("nvarchar(256)");
                                      c.NotNullable(true);
                                  }
                    ));

            Property(p => p.ModifiedByFullName,
                map => map.Column(c =>
                                  {
                                      c.Name("MODIFIEDBYFULLNAME");
                                      c.SqlType("nvarchar(256)");
                                      c.NotNullable(true);
                                  }
                    ));

            Property(p => p.ModifiedReason,
                map => map.Column(c =>
                                  {
                                      c.Name("MODIFIEDREASON");
                                      c.SqlType("nvarchar(1024)");
                                      c.NotNullable(false);
                                  }
                    ));
        }
    }
}
