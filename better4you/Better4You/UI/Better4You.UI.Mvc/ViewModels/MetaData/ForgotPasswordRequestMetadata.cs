using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Better4You.UI.Mvc.ViewModels.MetaData
{
    public class ForgotPasswordRequestMetadata
    {
        [DisplayName("Email Address")]
        [Required]
        public string UserName { get; set; }
    }
}