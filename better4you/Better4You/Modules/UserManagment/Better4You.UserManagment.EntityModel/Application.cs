using System.Collections.Generic;
using Better4You.Core.Repositories;
using Tar.Model;

namespace Better4You.UserManagment.EntityModel
{
    public class Application : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }        
        public virtual ICollection<User> Users { get; set; }
        public Application()
        {
            Users = new List<User>();
        }
        public virtual void AddUser(User newUser)
        {
            //newUser.Applications.Add(this);
            Users.Add(newUser);            
        }
    }
}