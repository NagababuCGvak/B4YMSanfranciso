using System.Collections.Generic;
using Tar.Security;
using Better4You.Core;

namespace Better4You.UI.Mvc.Core
{
    public static class Layouts
    {
        //public const string Default = @"~/Views/Shared/_Layout.cshtml";
        public const string Default = @"~/Views/Shared/_LoginLayout.cshtml";
        public const string Autorized = @"~/Views/Shared/_AutorizedLayout.cshtml";
        //public const string Company = @"~/Views/Shared/_CompanyLayout.cshtml";
        //public const string School = @"~/Views/Shared/_SchoolLayout.cshtml";         
        private static string LayoutFor(long userTypeId)
        {
            if (userTypeId>0)
                return Autorized;
            return Default;
        }

        public static string LayoutFor(CurrentUser currentUser)
        {
            return currentUser == null
                       ? Default
                       : Autorized;
        }
    }
}