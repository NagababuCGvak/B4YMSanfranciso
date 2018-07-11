using System.ServiceModel;
using Better4You.Meal.Service.Messages;
using Tar.Service.Messages;
using Tar.ViewModel;

namespace Better4You.Meal.Service
{
    [ServiceContract]
    public interface IMenuService
    {


                
        [OperationContract]
        MenuGetAllResponse GetAllByFilter(MenuGetAllRequest request);

        [OperationContract]
        MenuGetResponse Get(MenuGetRequest request);

        
        [OperationContract]
        MenuCreateResponse Create(MenuCreateRequest request);
        

        [OperationContract]
        MenuUpdateResponse Update(MenuUpdateRequest request);

        [OperationContract]
        VoidResponse Delete(MenuDeleteRequest request);
    }
}
