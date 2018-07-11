using System.Collections.Generic;
using Tar.Model;

namespace Better4You.EntityModel
{
    public class Country:IEntity
    {
        public virtual long Id { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<FirstAdminDivision> FirstAdminDivisions { get; set; }
        public virtual IList<City> Cities{ get; set; }
        public Country()
        {
            FirstAdminDivisions= new List<FirstAdminDivision>();
            Cities = new List<City>();
        }
    }
}