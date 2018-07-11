using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Better4You.UI.Mvc.ViewModels.MetaData
{
    public class ChangePasswordRequestMetadata
    {
        [Required]
        [DisplayName("Email Address")]
        public string UserName { get; set; }

        [HiddenInput]
        public string ActivationCode { get; set; }

        [Required]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password")]
        //[System.Web.Mvc.CompareAttribute("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}