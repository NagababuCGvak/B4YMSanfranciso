using System;
using System.Collections.Generic;
using Better4You.Core.Repositories;
using Better4You.UserManagment.EntityModel;

namespace Better4You.Meal.EntityModel
{
    public class Menu : IConfigEntity
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
        
        public virtual bool IsExclusive { get; set; }

        public virtual IList<Food> Foods { get; set; }

        public virtual long MenuType { get; set; }
        
        public virtual DateTime CreatedAt { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual string CreatedByFullName { get; set; }
        
        public virtual DateTime ModifiedAt { get; set; }
        
        public virtual string ModifiedBy { get; set; }
        
        public virtual string ModifiedByFullName { get; set; }
        
        public virtual string ModifiedReason { get; set; }

        public virtual long RecordStatus { get; set; }

        public virtual long SchoolType { get; set; }

        public virtual bool AdditionalFruit { get; set; }

        public virtual bool AdditionalVeg { get; set; }

        public virtual IList<School> Schools { get; set; }

        public Menu()
        {
            Foods = new List<Food>();
            Schools= new List<School>();
        }
        public virtual void AddFood(Food newFood)
        {
            Foods.Add(newFood);
        }
        public virtual void AddSchool(School newSchool)
        {
            Schools.Add(newSchool);
        }
    }
}