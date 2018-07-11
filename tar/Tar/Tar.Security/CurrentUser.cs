using System.Collections.Generic;
using System.Security.Principal;

namespace Tar.Security
{
    public class CurrentUser : IIdentity, IPrincipal
    {
        #region IIdentity
        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        /// <returns>
        /// The name of the user on whose behalf the code is running.
        /// </returns>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets value that indicates whether the user has been authenticated.
        /// </summary>
        /// <returns>
        /// true if the user was authenticated; otherwise, false.
        /// </returns>
        public bool IsAuthenticated { get; set; }


        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <returns>
        /// The type of authentication used to identify the user.
        /// </returns>
        public string AuthenticationType
        {
            get { return "Tar.Security"; }
        }
        #endregion
        #region IPrincipal
        public IIdentity Identity
        {
            get { return this; }
        }

        public bool IsInRole(string role)
        {
            var roles = Roles();
            return roles != null && roles.Contains(role);
        }
        #endregion

        public void Roles(List<string> roles)
        {
            Set("tar.security.roles", roles);
        }

        public List<string> Roles()
        {
            return Get<List<string>>("tar.security.roles");
        }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IDictionary<string, object> Data { get; set; }

        public void Set<T>(string key, T value)
        {
            Data[key] = value;
        }

        public T Get<T>(string key)
        {
            if (!Data.ContainsKey(key)) return default(T);

            return (T) Data[key];
        }

        public CurrentUser()
        {
            Data = new Dictionary<string, object>();
        }
    }
}