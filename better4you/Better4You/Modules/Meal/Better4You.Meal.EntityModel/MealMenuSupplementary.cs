using Better4You.Core.Repositories;
using Better4You.UserManagment.EntityModel;

namespace Better4You.Meal.EntityModel
{
    public class MealMenuSupplementary : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual School School { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual long MealType { get; set; }
        public virtual int Monday { get; set; }
        public virtual int Tuesday { get; set; }
        public virtual int Wednesday { get; set; }
        public virtual int Thursday { get; set; }
        public virtual int Friday { get; set; }
    }
}