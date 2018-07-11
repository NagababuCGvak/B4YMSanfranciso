using Better4You.Core.Repositories;

namespace Better4You.UserManagment.EntityModel
{
    public class SchoolAnnualAgreement : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual int Year { get; set; }
        public virtual double Price { get; set; }
        public virtual long RecordStatus { get; set; }
        public virtual School School { get; set; }
        //public virtual long MealType { get; set; }
        public virtual long ItemType { get; set; }

    }
}