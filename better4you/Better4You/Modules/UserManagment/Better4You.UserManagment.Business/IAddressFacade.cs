using System.Collections.Generic;
using Better4You.UserManagment.ViewModel;

namespace Better4You.UserManagment.Business
{
    public interface IAddressFacade
    {
        IList<AddressView> GetBySchoolId(int schoolId);
        AddressView GetById(int id);
        AddressView Save(AddressView view);
        bool Delete(int id);
    }

}
