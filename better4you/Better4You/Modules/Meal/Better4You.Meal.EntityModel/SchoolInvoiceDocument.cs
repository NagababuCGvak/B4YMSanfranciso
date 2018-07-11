using System;
using Better4You.Core.Repositories;

namespace Better4You.Meal.EntityModel
{
    public class SchoolInvoiceDocument : IConfigEntity
    {
        public virtual long Id { get; set; }

        public virtual long SchoolId { get; set; }

        public virtual int InvoiceYear { get; set; }

        public virtual int InvoiceMonth { get; set; }

        public virtual int Version { get; set; }
        
        public virtual DateTime CreatedAt { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual string CreatedByFullName { get; set; }

        public virtual DateTime ModifiedAt { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual string ModifiedByFullName { get; set; }

        public virtual string ModifiedReason { get; set; }

        public virtual long RecordStatus { get; set; }  

        public virtual string DocumentName { get; set; }

        public virtual byte[] DocumentStream { get; set; }

        public virtual Guid DocumentGuid { get; set; }

    }
}
