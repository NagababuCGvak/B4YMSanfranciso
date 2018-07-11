using System;
using Better4You.EntityModel;
using NHibernate.Mapping.ByCode;

namespace Better4You.NhCodeMapping
{
    public class EntityObjectMap//:IComponentMapper<EntityObject>
    {
        public static Action<IComponentMapper<EntityObject>> Mapping()
        {
            return k =>
                       {
                           k.Property(p => p.CreatedDate,
                                      map => map.Column(c =>
                                                            {
                                                                c.Name("CREATEDDATE");
                                                                c.SqlType("datetime");
                                                                c.NotNullable(true);
                                                            }
                                                 ));

                           k.Property(p => p.CreatedBy,
                                      map => map.Column(c =>
                                                            {
                                                                c.Name("CREATEDBY");
                                                                c.SqlType("int");
                                                                c.NotNullable(true);
                                                            }
                                                 ));
                           k.Property(p => p.ModifiedDate,
                                      map => map.Column(c =>
                                                            {
                                                                c.Name("MODIFIEDDATE");
                                                                c.SqlType("datetime");
                                                                c.NotNullable(true);
                                                            }
                                                 ));

                           k.Property(p => p.ModifiedBy,
                                      map => map.Column(c =>
                                                            {
                                                                c.Name("MODIFIEDBY");
                                                                c.SqlType("int");
                                                                c.NotNullable(true);
                                                            }
                                                 ));

                           k.Property(p => p.ModifiedReason,
                                      map => map.Column(c =>
                                                            {
                                                                c.Name("MODIFIEDREASON");
                                                                c.SqlType("nvarchar(1024)");
                                                                c.NotNullable(false);
                                                            }
                                                 ));
                           k.OneToOne(p=>p.RecordStatus,map=>
                                                            {
                                                                map.PropertyReference(typeof(RecordStatus).GetProperty("Id"));
                                                                map.Cascade(Cascade.None);
                                                            });

            };
        }
    }
}
