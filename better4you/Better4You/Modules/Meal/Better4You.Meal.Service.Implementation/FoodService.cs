using System;
using System.ServiceModel.Activation;
using Better4You.Meal.Business;
using Better4You.Meal.Service.Messages;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Implementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]    
    public class FoodService:Tar.Service.Service<IFoodService, FoodService>, IFoodService
    {        

        private readonly IFoodFacade _foodFacade;
        public FoodService(IFoodFacade foodFacade)
        {
            if (foodFacade == null) throw new ArgumentNullException("foodFacade");
            _foodFacade = foodFacade;

        }  
        public FoodGetAllResponse GetAllByFilter(FoodGetAllRequest request)
        {
            return Execute<FoodGetAllRequest, FoodGetAllResponse>(
                request,
                response =>
                    {
                        int totalCount;
                        response.Foods = _foodFacade.GetByFilter(request.Filter,
                                                                request.PageSize,
                                                                request.PageIndex,
                                                                request.OrderByField,
                                                                request.OrderByAsc, out totalCount);
                        response.TotalCount = totalCount;
                    });
        }
        public FoodGetResponse Get(FoodGetRequest request)
        {

            return Execute<FoodGetRequest, FoodGetResponse>(
                request,
                response =>
                {
                    response.Food = _foodFacade.Get(request.Id);
                }
                );
            
        }
        
        public FoodCreateResponse Create(FoodCreateRequest request)
        {
            return Execute<FoodCreateRequest, FoodCreateResponse>(
                request,
                response =>
                    {
                        response.Food = _foodFacade.Create(request.Food);
                    }
                );
        }
        
        public FoodUpdateResponse Update(FoodUpdateRequest request)
        {
            return Execute<FoodUpdateRequest, FoodUpdateResponse>(
                request,
                response =>
                    {
                        response.Food = _foodFacade.Update(request.Food);
                    }
                );

        }

        public VoidResponse Delete(FoodDeleteRequest request)
        {

            return Execute<FoodDeleteRequest, VoidResponse>(
                request,
                response => _foodFacade.Delete(request.Food));
            
        }

        public FoodPercentagesResponse GetFoodPercentages(FoodPercentagesRequest request)
        {
            return Execute<FoodPercentagesRequest, FoodPercentagesResponse>(
                request,
                response =>
                {
                    response.PercentageList=_foodFacade.GetFoodPercentages(request.SchoolId);
                });
        }

        public FoodPercentagesResponse SaveFoodPercentages(FoodPercentagesSaveRequest request)
        {
            return Execute<FoodPercentagesSaveRequest, FoodPercentagesResponse>(
                request,
                response =>
                {
                    response.PercentageList = _foodFacade.SaveFoodPercentages(request.PercentageList);
                });
        }
    }
}
