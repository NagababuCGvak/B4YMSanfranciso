using System.ServiceModel;
using Better4You.Meal.Service.Messages;
using Tar.Service.Messages;

namespace Better4You.Meal.Service
{
    [ServiceContract]
    public interface IFoodService
    {
        
        [OperationContract]
        FoodGetAllResponse GetAllByFilter(FoodGetAllRequest request);

        [OperationContract]
        FoodGetResponse Get(FoodGetRequest request);

        
        [OperationContract]
        FoodCreateResponse Create(FoodCreateRequest request);
        

        [OperationContract]
        FoodUpdateResponse Update(FoodUpdateRequest request);

        [OperationContract]
        VoidResponse Delete(FoodDeleteRequest request);

        [OperationContract]
        FoodPercentagesResponse GetFoodPercentages(FoodPercentagesRequest request);
        
        [OperationContract]
        FoodPercentagesResponse SaveFoodPercentages(FoodPercentagesSaveRequest request);

    }


}
