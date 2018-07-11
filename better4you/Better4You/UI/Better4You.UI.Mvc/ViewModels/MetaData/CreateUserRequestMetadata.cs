using System.Web.Mvc;

namespace Better4You.UI.Mvc.ViewModels.MetaData
{
    public class CreateUserRequestMetadata
    {
        public UserViewModelMetadata User { get; set; }

        [HiddenInput]
        public int ApplicationId { get; set; }
    }
}