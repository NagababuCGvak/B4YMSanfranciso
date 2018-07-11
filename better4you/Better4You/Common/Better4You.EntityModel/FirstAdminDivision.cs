using System.Collections.Generic;
using Better4You.Core.Repositories;

namespace Better4You.EntityModel
{
    public class FirstAdminDivision : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual string Name { get; set; }
        public virtual Country Country { get; set; }
        public virtual IList<City> Cities { get; set; } 
        public FirstAdminDivision()
        {
            Cities= new List<City>();
        }
    }
}