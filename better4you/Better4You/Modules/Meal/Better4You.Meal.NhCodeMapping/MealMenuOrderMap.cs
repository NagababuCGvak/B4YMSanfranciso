using Better4You.Meal.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class MealMenuOrderMap:ClassMapping<MealMenuOrder>
    {
        public MealMenuOrderMap()
        {
            Table("NTR_MEALMENUORDER");
            Lazy(true);

            Id(x => x.Id,
               map =>
               {
                   map.Column("MEALMENUORDERID");
                   map.Generator(Generators.Identity);
               });

            Property(x => x.TotalCredit,
                     map => map.Column(c =>
                                           {
                                               c.Name("TOTALCREDIT");
                                               c.SqlType("numeric(18,2)");
                                               c.NotNullable(false);
                                           }
                                )
                );
            
            Property(x => x.Rate,
                     map => map.Column(c =>
                                           {
                                               c.Name("RATE");
                                               c.SqlType("numeric(9,2)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.OrderDate,
                     map => map.Column(c =>
                                           {
                                               c.Name("ORDERDATE");
                                               c.SqlType("datetime");
                                               c.NotNullable(false);
                                           }
                                )
                );


            Property(p => p.OrderStatus,
                map =>
                    map.Column(c =>
                               {
                                   c.Name("ORDERSTATUS");
                                   c.SqlType("int");
                                   c.NotNullable(true);
                               })
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

            Bag(p => p.MealMenuOrderItems,
                map =>
                    {
                        map.Key(k =>
                                    {
                                        k.Column("MEALMENUORDERID");
                                        k.OnDelete(OnDeleteAction.NoAction);
                                    });
                        map.Cascade(Cascade.All);
                        map.Inverse(true);
                        map.Lazy(CollectionLazy.Lazy);
                    },
                ce => ce.OneToMany()
                );

            ManyToOne(p => p.School,
                map =>
                {
                    map.Column("SCHOOLID");
                    map.Lazy(LazyRelation.NoLazy);
                    map.NotNullable(true);
                }
                );

            Property(x => x.DebitAmount,
                map => map.Column(c =>
                                  {
                                      c.Name("DEBITAMOUNT");
                                      c.SqlType("numeric(18,2)");
                                      c.NotNullable(false);
                                  }
                    ));

            Property(x => x.Note,
                map => map.Column(c =>
                                  {
                                      c.Name("NOTE");
                                      c.SqlType("NVARCHAR(2048)");
                                      c.NotNullable(false);
                                  }
                    ));

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
