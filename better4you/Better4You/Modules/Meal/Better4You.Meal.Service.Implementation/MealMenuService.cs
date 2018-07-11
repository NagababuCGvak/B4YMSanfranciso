using System;
using System.ServiceModel.Activation;
using Better4You.Meal.Business;
using Better4You.Meal.Service.Messages;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Implementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]    
    public class MealMenuService:Tar.Service.Service<IMealMenuService, MealMenuService>, IMealMenuService
    {

        private readonly IMealMenuFacade _mealMenuFacade;
        public MealMenuService(IMealMenuFacade mealMenuFacade)
        {
            if (mealMenuFacade == null) throw new ArgumentNullException("mealMenuFacade");
            _mealMenuFacade = mealMenuFacade;

        }    

        public MealMenuGetAllResponse GetAllByFilter(MealMenuGetAllRequest request)
        {
            return Execute<MealMenuGetAllRequest, MealMenuGetAllResponse>(
                request,
                response =>
                    {
                        int totalCount;
                        response.MealMenus = _mealMenuFacade.GetByFilter(request.Filter,
                                                                request.PageSize,
                                                                request.PageIndex,
                                                                request.OrderByField,
                                                                request.OrderByAsc, out totalCount);
                        response.TotalCount = totalCount;
                    });
        }

        public MealMenuGetResponse Get(MealMenuGetRequest request)
        {
            return Execute<MealMenuGetRequest, MealMenuGetResponse>(
                request,
                response =>
                {
                    response.MealMenu = _mealMenuFacade.Get(request.Id);
                }
                );            
        }

        public MealMenuCreateResponse Create(MealMenuCreateRequest request)
        {
            return Execute<MealMenuCreateRequest, MealMenuCreateResponse>(
                request,
                response =>
                    {
                        response.MealMenu = _mealMenuFacade.Create(request.MealMenu);
                    }
                );
        }

        public MealMenuUpdateResponse Update(MealMenuUpdateRequest request)
        {
            return Execute<MealMenuUpdateRequest, MealMenuUpdateResponse>(
                request,
                response =>
                    {
                        response.MealMenu = _mealMenuFacade.Update(request.MealMenu);
                    }
                );

        }

        public MealMenuSaveResponse Save(MealMenuSaveRequest request)
        {
            return Execute<MealMenuSaveRequest, MealMenuSaveResponse>(
                request,
                response =>
                {
                    foreach (var mealMenu in request.MealMenus)
                    {
                        response.MealMenus.Add(_mealMenuFacade.Update(mealMenu));
                    }
                }
                );
        }
        public VoidResponse Delete(MealMenuDeleteRequest request)
        {

            return Execute<MealMenuDeleteRequest, VoidResponse>(
                request,
                response => _mealMenuFacade.Delete(request.MealMenu));
            
        }
    }
}
