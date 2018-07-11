using System;
using Better4You.Core.Repositories;

namespace Better4You.Meal.EntityModel
{
    public class MealMenu : IConfigEntity
    {
        public virtual long Id { get; set; }

        public virtual long MealType { get; set; }
        
        public virtual DateTime ValidDate { get; set; }
        
        public virtual Menu Menu { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual string CreatedByFullName { get; set; }

        public virtual DateTime ModifiedAt { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual string ModifiedByFullName { get; set; }

        public virtual string ModifiedReason { get; set; }

        public virtual long RecordStatus { get; set; }
    }
}
