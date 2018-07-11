using System;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Better4You.Core;
using Better4You.EntityModel;
using Better4You.Meal.Config;
using SysMngConfig = Better4You.UserManagement.Config;
using Better4You.UserManagment.Business.PasswordValidation;
using Better4You.UserManagment.EntityModel;
using Better4You.UserManagment.ViewModel;
using Tar.Core;

namespace Better4You.UserManagment.Business
{
    /// <summary>
    /// Map model to view, view to model
    /// </summary>
    public class BootStrapper:IBootStrapper
    {
        public void BootStrap()
        {
            Password();
            

            #region Application mappings ApplicationView
            Mapper.CreateMap<Application, ApplicationView>();
            Mapper.CreateMap<ApplicationView, Application>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion Application mappings ApplicationView

            #region User mappings UserView
            Mapper.CreateMap<User, UserView>();
            Mapper.CreateMap<UserView, User>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion User mappings UserView

            #region User mappings UserView
            Mapper.CreateMap<User, UserItemView>();
            Mapper.CreateMap<UserItemView, User>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion User mappings UserView

            #region UserLoginInfo mappings UserLoginInfoView
            Mapper.CreateMap<UserLoginInfo, UserLoginInfoView>();
            Mapper.CreateMap<UserLoginInfoView, UserLoginInfo>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion UserLoginInfo mappings UserLoginInfoView

            #region School mappings SchoolView
            Mapper.CreateMap<School, SchoolView>()
                .ForMember(c=>c.SchoolAnnualAgreements,opt=>opt.MapFrom(s=>s.SchoolAnnualAgreements.OrderByDescending(c=>c.Year).ThenBy(c=>c.ItemType)));

            Mapper.CreateMap<SchoolView, School>()
                .ForMember(c => c.Id, options => options.Ignore())
                .ForMember(c => c.SchoolRoutes, opt => opt.Ignore())
                .ForMember(c => c.SchoolAnnualAgreements, opt => opt.Ignore())
                .ForMember(c => c.Users, opt => opt.Ignore());

            Mapper.CreateMap<School, Tar.ViewModel.GeneralItemView>()
                .ConvertUsing(s => new Tar.ViewModel.GeneralItemView(s.Id, s.SchoolType.ToString(), string.Format("{0} ,#{1}", s.Name, SysMngConfig.Lookups.GetItem<SysMngConfig.SchoolTypes>(s.SchoolType).Text)));

            Mapper.CreateMap<School, GeneralItemView>()
                .ConvertUsing(s => new GeneralItemView(s.Id.ToString(CultureInfo.InvariantCulture), s.Name));
            Mapper.CreateMap<GeneralItemView, School>()
                .ConvertUsing(s => new School { Id = Convert.ToInt32(s.Value),Name = s.Text});


            Mapper.CreateMap<School, SchoolView>();

            Mapper.CreateMap<School, SchoolListItemView>()
                .ForMember(d => d.RecordStatus,
                    opt => opt.MapFrom(s => Lookups.GetItem<SysMngConfig.RecordStatuses>(s.RecordStatus).Text))
                .ForMember(d => d.SchoolType,
                    opt => opt.MapFrom(s => Lookups.GetItem<SysMngConfig.SchoolTypes>(s.SchoolType).Text))
                .ForMember(d => d.FoodServiceType,
                    opt => opt.MapFrom(s => Lookups.GetItem<SysMngConfig.FoodServiceType>(s.FoodServiceType).Text))
                .ForMember(d => d.OVSType,
                    opt => opt.MapFrom(s => Lookups.GetItem<SysMngConfig.OVSType>(s.OVSType).Text));


            #endregion School mappings SchoolView

            #region Communication mappings CommunicationView
            Mapper.CreateMap<Communication, CommunicationView>();
            Mapper.CreateMap<CommunicationView, Communication>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion Communication mappings CommunicationView

            #region Address mappings AddressView
            Mapper.CreateMap<Address, AddressView>();
            Mapper.CreateMap<AddressView, Address>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion Address mappings AddressView

            #region Role mappings RoleView
            Mapper.CreateMap<Role, RoleView>();
            Mapper.CreateMap<RoleView, Role>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion  Role mappings RoleView

            #region SchoolAnnualAgreement mappings SchoolAnnualAgreementView
            Mapper.CreateMap<SchoolAnnualAgreement, SchoolAnnualAgreementView>()
                //.ForMember(d => d.MealType, opt => opt.MapFrom(s => Lookups.GetItem<MealTypes>(s.MealType)))
                .ForMember(d => d.ItemType, opt => opt.MapFrom(s => Lookups.GetItem<AnnualItemTypes>(s.ItemType)))
                .ForMember(d => d.RecordStatus, opt => opt.MapFrom(s => SysMngConfig.Lookups.GetItem<SysMngConfig.RecordStatuses>(s.RecordStatus)));

            Mapper.CreateMap<SchoolAnnualAgreementView, SchoolAnnualAgreement>()
                .ForMember(c => c.Id, options => options.Ignore())
                //.ForMember(c => c.MealType, opt => opt.MapFrom(s => s.MealType.Id))
                .ForMember(c => c.ItemType, opt => opt.MapFrom(s => s.ItemType.Id))
                .ForMember(c => c.RecordStatus, opt => opt.MapFrom(s => s.RecordStatus.Id));


            #endregion  SchoolAnnualAgreement mappings SchoolAnnualAgreementView

            #region SchoolRoute mappings SchoolRouteView
            Mapper.CreateMap<SchoolRoute, SchoolRouteView>()
                .ForMember(d => d.MealType, opt => opt.MapFrom(s => Lookups.GetItem<MealTypes>(s.MealType)))
                .ForMember(d => d.RecordStatus, opt => opt.MapFrom(s => SysMngConfig.Lookups.GetItem<SysMngConfig.RecordStatuses>(s.RecordStatus)));
                
            Mapper.CreateMap<SchoolRouteView, SchoolRoute>()
                .ForMember(c => c.Id, options => options.Ignore())
                .ForMember(c => c.MealType, opt => opt.MapFrom(s => s.MealType.Id))
                .ForMember(c => c.RecordStatus, opt => opt.MapFrom(s => s.RecordStatus.Id));
            #endregion SchoolRoute mappings SchoolRouteView

            UserViewModelMap();
        }

        private void Password()
        {
            PasswordManager.Clear();
            PasswordManager.AddEncryptor("103001", new PlainTextPasswordEncryptor());
            PasswordManager.AddEncryptor("103002", new Md5PasswordEncryptor());
            PasswordManager.AddEncryptor("103003", new Sha1PasswordEncryptor());
        }

        private void UserViewModelMap()
        {

            Mapper.CreateMap<User, UserViewModel>()
                .ForMember(u => u.UserId, options => options.MapFrom(entity => entity.Id))
                .ForMember(u => u.IsLocked, options => options.MapFrom(entity => entity.UserLoginInfo.IsLocked));

            Mapper.CreateMap<long, UserType>().ConvertUsing<UserTypeIdUserTypeConverter>();
            Mapper.CreateMap<long, PasswordFormatType>().ConvertUsing<PasswordFormatTypeIdPasswordFormatTypeConverter>();


            //Mapper.CreateMap<UserViewModel, User>()
            //    .ForMember(user => user.Id, options => options.Ignore())
            //    .ForMember(user => user.UserType, options => options.MapFrom(x => x.UserTypeId))
            //    .ForMember(user => user.PasswordFormatType,
            //               options => options.MapFrom(x => new PasswordFormatType {Id = 103001}))
            //    .ForMember(user => user.AlgorithmType,
            //               options => options.MapFrom(x => new AlgorithmType {Id = 105001}))
            //    .ForMember(user => user.PasswordSalt, options => options.MapFrom(x => ""));
        }

        public class UserTypeIdUserTypeConverter : ITypeConverter<long, UserType>
        {
            public UserType Convert(ResolutionContext context)
            {
                var userTypeId = (long)context.SourceValue;
                return new UserType { Id = userTypeId };
            }
        }

        public class PasswordFormatTypeIdPasswordFormatTypeConverter : ITypeConverter<long, PasswordFormatType>
        {
            public PasswordFormatType Convert(ResolutionContext context)
            {
                var id = (long)context.SourceValue;
                return new PasswordFormatType {Id = id};
            }
        }
    }
}
