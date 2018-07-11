using System.ComponentModel;

namespace Better4You.UI.Mvc.ViewModels.MetaData
{
    public class LoginRequestMetadata
    {
        [DisplayName("Email Address")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }
    }
}