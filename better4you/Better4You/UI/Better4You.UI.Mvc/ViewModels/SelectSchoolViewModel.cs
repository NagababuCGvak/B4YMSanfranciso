using System;
using System.Linq;
using System.Web.Mvc;
using Better4You.Core;
using Better4You.UI.Mvc.Core;
using Tar.Security;

namespace Better4You.UI.Mvc.ViewModels
{
    public class SelectSchoolViewModel
    {
        public long ApplicationId { get; private set; }

        public long SchoolId { get; private set; }
        public SelectList ApplicationList { get; private set; }
        public SelectList SchoolList { get; private set; }
        public String ReturnUrl { get; private set; }
        public string Layout { get; private set; }

        public bool IsCompanyUser { get; private set; }
        public bool IsSchoolUser { get; private set; }

        public SelectSchoolViewModel(CurrentUser currentUser, string returnUrl)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            
            Layout = Layouts.LayoutFor(currentUser);
            ReturnUrl = returnUrl;
            
            if(string.IsNullOrEmpty(ReturnUrl))
                ReturnUrl = "~/Home/Index";

            ApplicationId = currentUser.CurrentApplicationId();
            SchoolId = currentUser.CurrentSchoolId();

            ApplicationList = currentUser.Applications().ToSelectList(ApplicationId);
            SchoolList = currentUser.Schools().ToSelectList(SchoolId);
            IsCompanyUser = currentUser.UserTypeId().IsCompanyUser();
            IsSchoolUser = currentUser.UserTypeId().IsSchoolUser();
        }


        public bool CanSelect()
        {
            if (IsCompanyUser && ApplicationList.Count() == 1)
                return false;

            if (IsSchoolUser && ApplicationList.Count() == 1 && SchoolList.Count() == 1)
                return false;

            return true;
        }
    }
}