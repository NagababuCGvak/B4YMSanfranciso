using System;
using Better4You.Core;
using Better4You.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.NhCodeMapping
{
    public abstract class LookupItemBaseMap<T> : ClassMapping<T> where T:LookupItem
    {
        //public int LookupGroupId { get { return (int) Enum.Parse(typeof (LookupGroups), typeof (T).Name); } }
        protected LookupItemBaseMap()
            : this(LookupGroups.None)
        {
        }

        protected LookupItemBaseMap(LookupGroups lookupGroup)
        {
            Table("GNRL_LOOKUPITEM");
            Lazy(true);
            Id(x => x.Id,
               map =>
                   {
                       map.Column("LOOKUPITEMID");
                       //map.Column("");
                       map.Generator(Generators.Assigned);
                   });

            Property(x => x.FieldValue,
                     map => map.Column(c =>
                                           {
                                               c.Name("FIELDVALUE");
                                               c.SqlType("nvarchar(64)");
                                               c.NotNullable(true);
                                           }
                                )
                );

            Property(x => x.FieldText,
                     map => map.Column(c =>
                                           {
                                               c.Name("FIELDTEXT");
                                               c.SqlType("nvarchar(256)");
                                               c.NotNullable(true);
                                           }
                                )
                );
            Property(x => x.Description,
                     map => map.Column(c =>
                                           {
                                               c.Name("DESCRIPTION");
                                               c.SqlType("nvarchar(2048)");
                                               c.NotNullable(true);
                                           }
                                )
                );

            Property(x => x.ItemOrder,
                     map => map.Column(c =>
                                           {
                                               c.Name("ITEMORDER");
                                               c.SqlType("int");
                                               c.NotNullable(true);
                                           }
                                )
                );
            Property(x => x.IsActive,
                     map => map.Column(c =>
                                           {
                                               c.Name("ISACTIVE");
                                               c.SqlType("bit");
                                               c.NotNullable(true);
                                               c.Default("1");
                                           }
                                )
                );

            ManyToOne(p => p.LookupGroup,
                      map =>
                          {
                              map.Column("LOOKUPGROUPID");
                              map.Lazy(LazyRelation.NoLazy);
                              map.NotNullable(true);
                          }
                );
            if (lookupGroup != LookupGroups.None)
                Where(string.Format("LOOKUPGROUPID={0}", Convert.ToInt32(lookupGroup)));
        }
    }
}