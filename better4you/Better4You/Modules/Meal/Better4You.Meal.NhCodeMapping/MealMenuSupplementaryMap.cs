using Better4You.Meal.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class MealMenuSupplementaryMap : ClassMapping<MealMenuSupplementary>
    {
        public MealMenuSupplementaryMap()
        {
            Table("NTR_MEALMENUSUPPLEMENTARY");
            Lazy(true);

            Id(x => x.Id,
                map =>
                {
                    map.Column("MEALMENUSUPPLEMENTARYID");
                    map.Generator(Generators.Identity);
                });

            ManyToOne(p => p.Menu,
                map =>
                {
                    map.Column("MENUID");
                    map.Lazy(LazyRelation.NoLazy);
                });
            ManyToOne(p => p.School,
                map =>
                {
                    map.Column("SCHOOLID");
                    map.Lazy(LazyRelation.NoLazy);
                });

            Property(x => x.MealType, map => map.Column(c => { c.Name("MEALTYPE"); }));

            //Mo Tu We Th Fr Sa
            Property(x => x.Monday, map => map.Column(c => { c.Name("MO"); }));
            Property(x => x.Tuesday, map => map.Column(c => { c.Name("TU"); }));
            Property(x => x.Wednesday, map => map.Column(c => { c.Name("WE"); }));
            Property(x => x.Thursday, map => map.Column(c => { c.Name("TH"); }));
            Property(x => x.Friday, map => map.Column(c => { c.Name("FR"); }));
        }
    }
}