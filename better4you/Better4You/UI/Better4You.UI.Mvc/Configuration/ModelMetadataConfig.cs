using System.Web.Mvc;
using Better4You.UI.Mvc.Core;
using Better4You.UI.Mvc.ViewModels;
using Better4You.UI.Mvc.ViewModels.MetaData;
using Better4You.UserManagment.Service.Messages;
using Better4You.UserManagment.ViewModel;

namespace Better4You.UI.Mvc.Configuration
{
    public class ModelMetadataConfig
    {
        public static void Configure()
        {
            var provider = ModelMetadataProviders.Current;
            provider.Add<SelectSchoolViewModel, SelectSchoolViewModelMetaData>();
            provider.Add<ResetPasswordRequest, ResetPasswordRequestMetadata>();
            provider.Add<CreateUserRequest, CreateUserRequestMetadata>();
            provider.Add<UserViewModel, UserViewModelMetadata>();
            provider.Add<LoginRequest, LoginRequestMetadata>();
            //provider.Add<ChangePasswordRequest, ChangePasswordRequestMetadata>();
            provider.Add<ForgotPasswordRequest, ForgotPasswordRequestMetadata>();
        }
    }
}