using Better4You.Meal.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class FoodMap:ClassMapping<Food>
    {
        public FoodMap()
        {
            Table("NTR_FOOD");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("FOODID");
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

            Property(p => p.FoodType,
                map =>
                    map.Column(c =>
                               {
                                   c.Name("FOODTYPE");
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

            Property(x => x.PortionSize,
                     map => map.Column(c =>
                                           {
                                               c.Name("PORTIONSIZE");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(true);
                                           }
                                )
                );

            Property(x => x.DisplayName,
                     map => map.Column(c =>
                                           {
                                               c.Name("DISPLAYNAME");
                                               c.SqlType("nvarchar(128)");
                                               c.NotNullable(true);
                                           }
                                )
                );
            Bag(p => p.FoodIngredients,
                map =>
                    {
                        map.Key(k =>
                                    {
                                        k.Column("FOODID");
                                        k.OnDelete(OnDeleteAction.NoAction);
                                    });
                        map.Cascade(Cascade.All);
                        map.Inverse(true);
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.OneToMany()
                );

            Bag(p => p.Menus,
                map =>
                {
                    map.Key(k => k.Column("FOODID"));
                    map.Table("NTR_MENU_FOOD");
                    map.Lazy(CollectionLazy.Lazy);
                },
                ce => ce.ManyToMany(l => { l.Column("MENUID"); l.Class(typeof(Menu)); })
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
