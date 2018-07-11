using System.ServiceModel;
using Better4You.Meal.Service.Messages;
using Tar.Service.Messages;
using Tar.ViewModel;

namespace Better4You.Meal.Service
{
    [ServiceContract]
    public interface IMealMenuService
    {


        [OperationContract]
        MealMenuGetAllResponse GetAllByFilter(MealMenuGetAllRequest request);

        [OperationContract]
        MealMenuGetResponse Get(MealMenuGetRequest request);

        
        [OperationContract]
        MealMenuCreateResponse Create(MealMenuCreateRequest request);
        

        [OperationContract]
        MealMenuUpdateResponse Update(MealMenuUpdateRequest request);

        [OperationContract]
        MealMenuSaveResponse Save(MealMenuSaveRequest request);
        
        [OperationContract]
        VoidResponse Delete(MealMenuDeleteRequest request);
    }
}
