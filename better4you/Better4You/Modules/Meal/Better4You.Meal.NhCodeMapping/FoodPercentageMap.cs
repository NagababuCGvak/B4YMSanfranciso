using Better4You.Meal.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class FoodPercentageMap : ClassMapping<FoodPercentage>
    {
        public FoodPercentageMap()
        {
            Table("NTR_FOODPERCENTAGE");
            Lazy(true);

            Id(x => x.Id,
                map =>
                {
                    map.Column("FOODPERCENTAGEID");
                    map.Generator(Generators.Identity);
                });

            Property(x => x.MealType, map => map.Column(c => { c.Name("MEALTYPE"); }));
            Property(x => x.SchoolId, map => map.Column(c => { c.Name("SCHOOLID"); }));
            Property(x => x.Fruit, map => map.Column(c => { c.Name("FRUIT"); }));
            Property(x => x.Vegetable, map => map.Column(c => { c.Name("VEGETABLE"); }));
        }
    }
}