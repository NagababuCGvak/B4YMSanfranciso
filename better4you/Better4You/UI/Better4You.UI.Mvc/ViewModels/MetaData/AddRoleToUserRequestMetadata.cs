using System.ComponentModel;

namespace Better4You.UI.Mvc.ViewModels.MetaData
{
    public class AddRoleToUserRequestMetadata
    {
        [DisplayName("User")]
        public int UserId { get; set; }

        [DisplayName("Role")]
        public int RoleId { get; set; }
    }
}