using Better4You.Core.Repositories;

namespace Better4You.EntityModel
{
    public class City:IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Country Country { get; set; }
        public virtual FirstAdminDivision FirstAdminDivision { get; set; }
    }
}