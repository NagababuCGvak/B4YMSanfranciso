using System;
using System.Collections.Generic;
using Better4You.Core.Repositories;
using Better4You.UserManagment.EntityModel;

namespace Better4You.Meal.EntityModel
{
    public class MealMenuOrder : IConfigEntity
    {
        public virtual long Id { get; set; }

        public virtual School School { get; set; }
        
        public virtual double? TotalCredit { get; set; }
        
        public virtual double? Rate { get; set; }
        
        public virtual long OrderStatus { get; set; }
        
        public virtual DateTime OrderDate { get; set; }
        
        public virtual DateTime CreatedAt { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual string CreatedByFullName { get; set; }

        public virtual DateTime ModifiedAt { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual string ModifiedByFullName { get; set; }

        public virtual string ModifiedReason { get; set; }

        public virtual long RecordStatus { get; set; }  

        public virtual IList<MealMenuOrderItem> MealMenuOrderItems { get; set; }

        public virtual long MealType { get; set; }
        
        public virtual double? DebitAmount { get; set; }
        
        public virtual string Note { get; set; }
        
        public MealMenuOrder()
        {
            MealMenuOrderItems= new List<MealMenuOrderItem>();
        }

        public virtual void AddMealMenuOrderItem(MealMenuOrderItem newMealMenuOrderItem)
        {
            newMealMenuOrderItem.MealMenuOrder = this;
            MealMenuOrderItems.Add(newMealMenuOrderItem);
        }
    }
}