using System.Collections.Generic;
using Better4You.Core.Repositories;
using Tar.Model;

namespace Better4You.UserManagment.EntityModel
{
    public class Role : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual IList<User> Users { get; set; }
        public Role()
        {
            Users = new List<User>();
        }
    }
}