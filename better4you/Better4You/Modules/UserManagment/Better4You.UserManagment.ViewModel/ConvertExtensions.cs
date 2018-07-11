using System.Linq;
using Better4You.Core;
using Better4You.UserManagment.EntityModel;

namespace Better4You.UserManagment.ViewModel
{
    public static class ConvertExtensions
    {
        public static UserViewModel ToViewModel(this User user)
        {
            if (user == null) return null;
            var userViewModel = AutoMapper.Mapper.Map<User, UserViewModel>(user);
            //var email = user.Communications.FirstOrDefault(
            //    c => c.IsPrimary && c.CommunicationType.Id == (int)CommunicationTypes.Email);

            //if (email != null) userViewModel.Email = email.Email;

            var phone = user.Communications.FirstOrDefault(
                c => c.IsPrimary && c.CommunicationType.Id == (long)CommunicationTypes.HomePhone)
                        ?? user.Communications.FirstOrDefault(
                            c => c.IsPrimary && c.CommunicationType.Id == (long)CommunicationTypes.WorkPhone);

            if (phone != null) userViewModel.Phone = phone.Phone;
            userViewModel.ActivitationCode = user.UserLoginInfo.ActivationCode;
            if(user.UserLoginInfo.ExpireActivationDate.HasValue)
                userViewModel.ExpireActivationDate = user.UserLoginInfo.ExpireActivationDate.Value;

            return userViewModel;
        }

        public static ApplicationView ToView(this Application application)
        {
            return AutoMapper.Mapper.Map<Application, ApplicationView>(application);
        }
    }
}
