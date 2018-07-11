using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace Better4You.UI.Mvc.ViewModels.MetaData
{
    public class SelectSchoolViewModelMetaData
    {
        [DisplayName("Company")]
        public int ApplicationId { get; private set; }

        [DisplayName("School")]
        public int SchoolId { get; private set; }

        public SelectList ApplicationList { get; private set; }
        public SelectList SchoolList { get; private set; }
        public String ReturnUrl { get; private set; }
        public string Layout { get; private set; }

        public bool IsCompanyUser { get; private set; }
        public bool IsSchoolUser { get; private set; }
    }
}