using System;
using System.Threading;
using System.Web;
using System.Web.Security;
using Tar.Core;

namespace Tar.Security
{
    public class AuthenticationModule : IHttpModule
    {
        private HttpApplication _context;

        #region Implementation of IHttpModule

        public void Init(HttpApplication context)
        {
            if (context == null) throw new ArgumentNullException("context");
            context.AuthenticateRequest += AuthenticateRequest;
            _context = context;
        }

        void AuthenticateRequest(object sender, EventArgs e)
        {
            if (_context.User.Identity.IsAuthenticated)
            {
                if (_context.User.Identity.IsAuthenticated)
                {
                    var id = _context.User.Identity as FormsIdentity;
                    if (id != null)
                    {
                        var currentUser = ServiceLocator.Current.Get<CurrentUser>();
                        if (!currentUser.IsAuthenticated)
                        {
                            currentUser.IsAuthenticated = true;
                            Thread.CurrentPrincipal = currentUser;
                            HttpContext.Current.User = Thread.CurrentPrincipal;
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
