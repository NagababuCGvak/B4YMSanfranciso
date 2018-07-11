using System.Collections.Generic;
using Better4You.Core.Repositories;

namespace Better4You.UserManagment.EntityModel
{
    public class School : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }

        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }

        public virtual long CityId { get; set; }

        public virtual string FirstAdminDivision { get; set; }

        public virtual long FirstAdminDivisionId { get; set; }

        public virtual string ZipCode { get; set; }

        public virtual string BusinessPhone { get; set; }

        public virtual string CellPhone { get; set; }

        public virtual string Fax { get; set; }
        
        public virtual string Email { get; set; }

        public virtual string Notes { get; set; }

        public virtual long SchoolType { get; set; }

        public virtual long OVSType { get; set; }

        public virtual long FoodServiceType { get; set; }

        public virtual long RecordStatus { get; set; }



        public virtual IList<User> Users { get; set; }
        public virtual IList<SchoolAnnualAgreement> SchoolAnnualAgreements { get; set; }
        public virtual IList<SchoolRoute> SchoolRoutes { get; set; }
        
        public School()
        {

            Users = new List<User>();
            SchoolAnnualAgreements = new List<SchoolAnnualAgreement>();
            SchoolRoutes = new List<SchoolRoute>();
        }

        public virtual void AddUser(User newUser)
        {
            //newAddress.Schools.Add(this);
            Users.Add(newUser);
        }
        public virtual void AddSchoolAnnualAgreement(SchoolAnnualAgreement newSchoolAnnualAgreement)
        {
            newSchoolAnnualAgreement.School=this;
            SchoolAnnualAgreements.Add(newSchoolAnnualAgreement);
        }
        public virtual void AddSchoolRoute(SchoolRoute newSchoolRoute)
        {
            newSchoolRoute.School = this;
            SchoolRoutes.Add(newSchoolRoute);
        }

    }
}
