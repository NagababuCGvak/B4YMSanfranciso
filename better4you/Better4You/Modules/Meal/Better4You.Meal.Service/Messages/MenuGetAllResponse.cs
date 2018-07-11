using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MenuGetAllResponse: PageableResponse
    {
        [DataMember]
        public List<MenuListItemView> Menus { get; set; }

        public MenuGetAllResponse()
        {
            Menus = new List<MenuListItemView>();
        }
    }
}