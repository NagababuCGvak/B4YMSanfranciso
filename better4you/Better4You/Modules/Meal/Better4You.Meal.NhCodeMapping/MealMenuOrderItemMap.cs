using Better4You.Meal.EntityModel;
using Better4You.NhCodeMapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class MealMenuOrderItemMap : ClassMapping<MealMenuOrderItem>
    {
        public MealMenuOrderItemMap()
        {
            Table("NTR_MEALMENUORDERITEM");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("MEALMENUORDERITEMID");
                   map.Generator(Generators.Identity);
               });

            Property(x => x.TotalCount,
                     map => map.Column(c =>
                                           {
                                               c.Name("TOTALCOUNT");
                                               c.SqlType("int");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Rate,
                     map => map.Column(c =>
                                           {
                                               c.Name("RATE");
                                               c.SqlType("numeric(18,2)");
                                               c.NotNullable(false);
                                           }
                                )
                );


            Property(x => x.AdjusmentCount,
                     map => map.Column(c =>
                                           {
                                               c.Name("ADJUSMENTCOUNT");
                                               c.SqlType("int");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Version,
                     map => map.Column(c =>
                                           {
                                               c.Name("VERSION");
                                               c.SqlType("int");
                                               c.Default(0);
                                               c.NotNullable(true);
                                           }
                                )
                );
            Property(x => x.RefId,
                     map => map.Column(c =>
                                           {
                                               c.Name("MEALMENUORDERITEMREFID");
                                               c.SqlType("int");
                                               c.NotNullable(false);
                                           }
                                )
                );


            ManyToOne(p => p.MealMenuOrder,
                      map =>
                      {
                          map.Column("MEALMENUORDERID");
                          map.Lazy(LazyRelation.NoLazy);
                      }
                );

            ManyToOne(p => p.MealMenu,
                      map =>
                      {
                          map.Column("MEALMENUID");
                          map.Lazy(LazyRelation.NoLazy);
                      }
                );

            Property(p => p.MealServiceType,
                map =>
                    map.Column(c =>
                               {
                                   c.Name("MEALSERVICETYPE");
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
