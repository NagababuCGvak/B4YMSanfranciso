using Better4You.EntityModel;
using Better4You.UserManagment.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class SchoolAnnualAgreementMap:ClassMapping<SchoolAnnualAgreement>
    {
        public SchoolAnnualAgreementMap()
        {
            Table("UM_SCHOOLANNUALAGREEMENT");
            Lazy(true);
            Id(x => x.Id,
               map =>
               {
                   map.Column("SCHOOLANNUALAGREEMENTID");
                   map.Generator(Generators.Native);
               });

            Property(x => x.Year,
                     map => map.Column(c =>
                                           {
                                               c.Name("ANNUALYEAR");
                                               c.SqlType("int");
                                               c.NotNullable(false);
                                           }
                                )
                );

            Property(x => x.Price,
                     map => map.Column(c =>
                                           {
                                               c.Name("PRICE");
                                               c.SqlType("decimal(9, 2)");
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
            Property(x => x.ItemType,
                map => map.Column(c =>
                {
                    c.Name("ITEMTYPE");
                    c.SqlType("int");
                    c.NotNullable(false);
                }
            ));
            /*
            Property(x => x.MealType,
                     map => map.Column(c =>
                     {
                         c.Name("MEALTYPE");
                         c.SqlType("int");
                         c.NotNullable(false);
                     }
            ));
            */
        }
    }
}
