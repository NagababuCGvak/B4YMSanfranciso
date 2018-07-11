using System.Collections.Generic;
using Better4You.Core.Repositories;
using Better4You.EntityModel;
using Tar.Model;

namespace Better4You.UserManagment.EntityModel
{
    public class Communication:IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual CommunicationType CommunicationType{ get; set; }
        public virtual string Phone { get; set; }
        public virtual string Extension { get; set; }
        public virtual string Email { get; set; }
        public virtual bool IsPrimary { get; set; }
        public virtual string Description { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
        public virtual IList<User> Users { get; set; }
        public virtual IList<School> Schools { get; set; }

        public Communication()
        {
            Users = new List<User>();
            Schools= new List<School>();

        }
    }
}