using System;
using Better4You.Core.Repositories;

namespace Better4You.Meal.EntityModel
{
    public class MealMenuOrderItem : IConfigEntity
    {
        public virtual long Id { get; set; }
        
        public virtual MealMenu MealMenu { get; set; }
        
        public virtual int? TotalCount { get; set; }

        public virtual double? Rate { get; set; }

        public virtual int? AdjusmentCount { get; set; }

        public virtual long? RefId { get; set; }
        
        public virtual int Version { get; set; }
        
        public virtual DateTime CreatedAt { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual string CreatedByFullName { get; set; }

        public virtual DateTime ModifiedAt { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual string ModifiedByFullName { get; set; }

        public virtual string ModifiedReason { get; set; }

        public virtual long RecordStatus { get; set; }  

        public virtual MealMenuOrder MealMenuOrder { get; set; }

        public virtual long MealServiceType { get; set; }
    }
}