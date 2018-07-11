using System.Collections.Generic;
using Better4You.Core.Repositories;
using Better4You.EntityModel;

namespace Better4You.UserManagment.EntityModel
{
    public class User:IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual PasswordFormatType PasswordFormatType { get; set; }
        public virtual string Password { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual AlgorithmType AlgorithmType { get; set; }
        public virtual string UserName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }


        public virtual IList<Application> Applications { get; set; }
        public virtual UserLoginInfo UserLoginInfo { get; protected set; }
        public virtual IList<Role> Roles { get; set; }
        public virtual IList<School> Schools { get; set; }
        public virtual IList<Communication> Communications { get; set; }

        public User()
        {
            Roles = new List<Role>();
            Schools= new List<School>();
            Communications = new List<Communication>();
            Applications = new List<Application>();
            //UserLoginInfo = new UserLoginInfo {User = this};
        }

        public virtual UserLoginInfo SetUserLoginInfo(UserLoginInfo newUserLoginInfo)
        {
            newUserLoginInfo.User = this;
            UserLoginInfo = newUserLoginInfo;
            return UserLoginInfo;
        }
        public virtual void AddApplication(Application newApplication)
        {
            //newApplication.Users.Add(this);
            Applications.Add(newApplication);
        }
        public virtual void AddRoles(Role newRole)
        {
            //newRole.Users.Add(this);
            Roles.Add(newRole);
        }
        public virtual void AddSchool(School newSchool)
        {
            //newSchool.Users.Add(this);
            Schools.Add(newSchool);
        }
        public virtual void AddCommunication(Communication newCommunication)
        {
            //newCommunication.Users.Add(this);
            Communications.Add(newCommunication);
        }
    }
}