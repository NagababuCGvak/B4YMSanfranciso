using Better4You.Meal.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class MealMenuOrderDayMap : ClassMapping<MealMenuOrderDay>
    {
        public MealMenuOrderDayMap()
        {
            Table("NTR_MEALMENUORDERDAY");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("MEALMENUORDERDAYID");
                   map.Generator(Generators.Identity);
               });

            Property(x => x.DeliveryType, map => map.Column(c => { c.Name("DELIVERYTYPE"); }));
            Property(x => x.ValidDate, map => map.Column(c => { c.Name("VALIDDATE"); }));

            Property(x => x.FruitCount,
                map => map.Column(c =>
                {
                    c.Name("FRUITCOUNT");
                    c.SqlType("int");
                    c.NotNullable(false);
                }
                    )
                );

            Property(x => x.VegetableCount,
                map => map.Column(c =>
                {
                    c.Name("VEGETABLECOUNT");
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
                });
        }
    }
}
