using System.Collections.Generic;
using Better4You.Core.Repositories;
using Better4You.EntityModel;
using Tar.Model;

namespace Better4You.UserManagment.EntityModel
{
    public class Address : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string District { get; set; }
        public virtual City City { get; set; }
        public virtual FirstAdminDivision FirstAdminDivision { get; set; }
        public virtual Country Country { get; set; }
        public virtual AddressType AddressType { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
        public virtual IList<School> Schools { get; set; }

        public Address()
        {
            Schools= new List<School>();
        }

    }
}