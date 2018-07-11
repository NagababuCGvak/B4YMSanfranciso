using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Better4You.UserManagment.Business;
using Tar.Security;
using Better4You.Core;

namespace Better4You.UserManagment.Service.Implementation
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IUserFacade _userFacade;

        public AuthenticationProvider(IUserFacade userFacade)
        {
            if (userFacade == null) throw new ArgumentNullException("userFacade");
            _userFacade = userFacade;
        }

        #region Implementation of IAuthenticationProvider
        public CurrentUser GetUser(string name, string password)
        {
            return GetUser(name);
        }

        public CurrentUser GetUser(string name)
        {
            var currentUser = new CurrentUser();
            var userViewModel = _userFacade.GetUser(name);
            if (userViewModel == null) return currentUser;

            currentUser.FullName = userViewModel.FirstName + " " + userViewModel.LastName;
            currentUser.Name = name;
            currentUser.UserTypeId(userViewModel.UserTypeId);
            currentUser.Data.Add("UserId", userViewModel.UserId);
            currentUser.Data.Add("FirstName", userViewModel.FirstName);
            currentUser.Data.Add("LastName", userViewModel.LastName); 
            var userApplications = _userFacade.GetUserApplications(userViewModel.UserId);
            currentUser.Applications(userApplications);

            var userSchools = _userFacade.GetUserSchools(userViewModel.UserId);
            currentUser.Schools(userSchools);

            if (currentUser.Applications().Count == 1)
                currentUser.CurrentApplicationId(
                    Convert.ToInt32(currentUser.Applications().First().Value));

            if (currentUser.Schools().Count == 1)
            {
                var school=currentUser.Schools().First();
                currentUser.CurrentSchoolId(Convert.ToInt32(school.Id));
            }
            var userRoles = _userFacade.GetUserRoles(userViewModel.UserId);
            currentUser.Roles(userRoles.Select(x => x.Value).ToList());

            return currentUser;
        }
        #endregion
    }
}
