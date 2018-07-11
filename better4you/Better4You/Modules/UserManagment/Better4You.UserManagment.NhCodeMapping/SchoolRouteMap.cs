using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class SchoolRouteMap:ClassMapping<SchoolRoute>
    {
        public SchoolRouteMap()
        {

            Table("UM_SCHOOLROUTE");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("SCHOOLROUTEID");
                   map.Generator(Generators.Native);
               });

            Property(x => x.Route,
                     map => map.Column(c =>
                                           {
                                               c.Name("ROUTE");
                                               c.SqlType("NVARCHAR(32)");
                                               c.NotNullable(false);
                                           }
                                )
                );
            
            Property(x => x.MealType,
                     map => map.Column(c =>
                                           {
                                               c.Name("MEALTYPE");
                                               c.SqlType("int");
                                               c.NotNullable(false);
                                           }
                                )
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

            ManyToOne(p => p.School,
                map =>
                {
                    map.Column("SCHOOLID");
                    map.Lazy(LazyRelation.NoLazy);
                }
            );
        }
    }
}
