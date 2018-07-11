using Better4You.Meal.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class FoodIngredientMap:ClassMapping<FoodIngredient>
    {
        public FoodIngredientMap()
        {
            Table("NTR_FOODINGREDIENT");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("FOODINGREDIENTID");
                   map.Generator(Generators.Identity);
               });

            Property(x => x.Description,
                     map => map.Column(c =>
                                           {
                                               c.Name("DESCRIPTION");
                                               c.SqlType("nvarchar(512)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Value,
                     map => map.Column(c =>
                                           {
                                               c.Name("VALUE");
                                               c.SqlType("numeric(9,2)");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(p => p.IngredientType,
                map =>
                    map.Column(c =>
                               {
                                   c.Name("INGREDIENTTYPE");
                                   c.SqlType("int");
                                   c.NotNullable(true);
                               })
                );

            ManyToOne(p => p.Food,
                      map =>
                          {
                              map.Column("FOODID");
                              map.Lazy(LazyRelation.NoLazy);
                          }
                );


            
        }
    }
}
