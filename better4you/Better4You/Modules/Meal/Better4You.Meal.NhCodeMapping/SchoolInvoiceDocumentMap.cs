using Better4You.Meal.EntityModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Better4You.Meal.NhCodeMapping
{
    public class SchoolInvoiceDocumentMap : ClassMapping<SchoolInvoiceDocument>
    {
        public SchoolInvoiceDocumentMap()
        {
            Table("NTR_SCHOOLINVOICEDOCUMENT");
            Lazy(true);
            Id(x => x.Id,
                map =>
                {
                    map.Column("SCHOOLINVOICEDOCUMENTID");
                    map.Generator(Generators.Identity);
                });

            Property(x => x.SchoolId,
                map => map.Column(c =>
                {
                    c.Name("SCHOOLID");
                    c.SqlType("int");
                    c.NotNullable(true);
                })
            );


            Property(x => x.InvoiceYear,
                map => map.Column(c =>
                {
                    c.Name("INVOICEYEAR");
                    c.SqlType("int");
                    c.NotNullable(true);
                })
            );

            Property(x => x.InvoiceMonth,
                map => map.Column(c =>
                {
                    c.Name("INVOICEMONTH");
                    c.SqlType("int");
                    c.NotNullable(true);
                })
            );

            Property(x => x.Version,
                map => map.Column(c =>
                {
                    c.Name("VERSION");
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

            Property(x => x.DocumentName,
                map => map.Column(c =>
                {
                    c.Name("DOCUMENTNAME");
                    c.SqlType("nvarchar(500)");
                    c.NotNullable(true);
                })
            );

            Property(x => x.DocumentStream,
                map => map.Column(c =>
                {
                    c.Name("DOCUMENTSTREAM");
                    c.SqlType("VARBINARY(MAX)");
                    c.Length(int.MaxValue);
                    c.NotNullable(true);
                })
            );

            Property(x => x.DocumentGuid,
                map => map.Column(c =>
                {
                    c.Name("DOCUMENTGUID");
                    c.SqlType("uniqueidentifier");
                    c.NotNullable(true);
                })
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
