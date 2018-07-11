using System.Collections.Generic;
using Tar.Model;

namespace Better4You.EntityModel
{
    public class LookupGroup : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<LookupItem> LookupItems { get; set; }
        public LookupGroup()
        {
            LookupItems= new List<LookupItem>();
        }
    }
}