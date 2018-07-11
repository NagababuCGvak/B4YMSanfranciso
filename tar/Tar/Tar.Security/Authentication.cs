using System;
using System.Threading;
using System.Web;

namespace Tar.Security
{
    public class Authentication : IAuthentication
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        
        public Authentication(IAuthenticationProvider authenticationProvider)
        {
            if (authenticationProvider == null) throw new ArgumentNullException("authenticationProvider");
            _authenticationProvider = authenticationProvider;
        }

        public virtual CurrentUser SignIn(string userName, string password)
        {
            var userIdentity = _authenticationProvider.GetUser(userName, password);
            userIdentity.IsAuthenticated = true;

            SetPrinciple(userIdentity);

            return userIdentity;
        }

        public virtual void SignOut()
        {
            SetPrinciple(new CurrentUser {IsAuthenticated = false});
        }

        private void SetPrinciple(CurrentUser currentUser)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");

            Thread.CurrentPrincipal = currentUser;
            if (HttpContext.Current != null) HttpContext.Current.User = currentUser;
        }
    }
}