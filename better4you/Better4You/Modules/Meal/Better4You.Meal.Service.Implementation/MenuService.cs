using System;
using System.ServiceModel.Activation;
using Better4You.Meal.Business;
using Better4You.Meal.Service.Messages;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Implementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]    
    public class MenuService:Tar.Service.Service<IMenuService, MenuService>, IMenuService
    {
        private readonly IMenuFacade _menuFacade;
        public MenuService(IMenuFacade menuFacade)
        {
            if (menuFacade == null) throw new ArgumentNullException("menuFacade");
            _menuFacade = menuFacade;

        }        

        public MenuGetAllResponse GetAllByFilter(MenuGetAllRequest request)
        {
            return Execute<MenuGetAllRequest, MenuGetAllResponse>(
                request,
                response =>
                    {
                        int totalCount;
                        response.Menus = _menuFacade.GetByFilter(request.Filter,
                                                                request.PageSize,
                                                                request.PageIndex,
                                                                request.OrderByField,
                                                                request.OrderByAsc, out totalCount);
                        response.TotalCount = totalCount;
                    });
        }
        public MenuGetResponse Get(MenuGetRequest request)
        {

            return Execute<MenuGetRequest, MenuGetResponse>(
                request,
                response =>
                {
                    response.Menu = _menuFacade.Get(request.Id);
                }
                );
            
        }

        public MenuCreateResponse Create(MenuCreateRequest request)
        {
            return Execute<MenuCreateRequest, MenuCreateResponse>(
                request,
                response =>
                    {
                        response.Menu = _menuFacade.Create(request.Menu);
                    }
                );
        }

        public MenuUpdateResponse Update(MenuUpdateRequest request)
        {
            return Execute<MenuUpdateRequest, MenuUpdateResponse>(
                request,
                response =>
                    {
                        response.Menu = _menuFacade.Update(request.Menu);
                    }
                );

        }

        public VoidResponse Delete(MenuDeleteRequest request)
        {

            return Execute<MenuDeleteRequest, VoidResponse>(
                request,
                response => _menuFacade.Delete(request.Menu));
            
        }
    }
}
