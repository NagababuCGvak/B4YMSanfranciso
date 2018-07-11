using Better4You.Meal.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class MealMenuMap:ClassMapping<MealMenu>
    {
        public MealMenuMap()
        {
            Table("NTR_MEALMENU");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("MEALMENUID");
                   map.Generator(Generators.Identity);
               });

            Property(x => x.ValidDate,
                     map => map.Column(c =>
                                           {
                                               c.Name("VALIDDATE");
                                               c.SqlType("datetime");
                                               c.NotNullable(true);
                                           }
                                )
                );
            

            Property(p => p.MealType,
                map =>
                    map.Column(c =>
                               {
                                   c.Name("MEALTYPE");
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

            ManyToOne(p => p.Menu,
                      map =>
                          {
                              map.Column("MENUID");
                              map.Lazy(LazyRelation.NoLazy);
                              map.Cascade(Cascade.None);
                              map.NotNullable(true);
                          }
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
