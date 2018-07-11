using Better4You.Core.Repositories;
using Better4You.EntityModel;
using Tar.Model;

namespace Better4You.UserManagment.EntityModel
{
    public class SchoolRoute : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual string Route { get; set; }
        public virtual School School { get; set; }
        public virtual long MealType { get; set; }
        public virtual long RecordStatus { get; set; }

    }
}
