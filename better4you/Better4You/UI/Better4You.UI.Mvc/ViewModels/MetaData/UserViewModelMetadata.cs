using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Better4You.UI.Mvc.ViewModels.MetaData
{
    public class UserViewModelMetadata
    {
        [HiddenInput]
        public int UserId { get; set; }

        [Required]
        [DisplayName("User Type")]
        [UIHint("UserTypeIdDropDownList")]
        public int UserTypeId { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //[Required]
        //public string Email { get; set; }

        [Required]
        public string Phone { get; set; }
    }

}