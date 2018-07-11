using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace Tar.Security
{
    public static class SecurityExtensions
    {
        public static CurrentUser AsCurrentUser(this IPrincipal principal)
        {
            return (CurrentUser)Thread.CurrentPrincipal;
        }

        public static void SetTo(this CurrentUser source, CurrentUser target)
        {
            source.Data.ToList().ForEach(item => target.Data[item.Key] = item.Value);
            target.FullName = source.FullName;
            target.IsAuthenticated = source.IsAuthenticated;
            target.Name = source.Name;
        }
    }
}
