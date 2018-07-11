using AutoMapper;
using Better4You.UserManagment.EntityModel;
using Better4You.UserManagment.ViewModel;

namespace Better4You.UserManagment.Business
{
    public static class MappingExtension
    {

        #region User
        public static UserItemView ToView(this User model)
        {
            return Mapper.Map<UserItemView>(model);
        }
        public static User ToModel(this UserItemView view)
        {
            return Mapper.Map<User>(view);
        }
        public static void SetTo(this UserItemView view, User model)
        {
            Mapper.Map(view, model);
        }
        #endregion User

        #region School
        /*
        public static SchoolView ToView(this School model)
        {
            return Mapper.Map<SchoolView>(model);
        }
        public static T ToView<T>(this School model)
        {
            return Mapper.Map<T>(model);
        }
        public static School ToModel(this SchoolView view)
        {
            return Mapper.Map<School>(view);
        }

        public static void SetTo(this SchoolView view, School model)
        {
            Mapper.Map(view, model);
        }
        */ 
        #endregion School

        #region Communication

        public static CommunicationView ToView(this Communication model)
        {
            return Mapper.Map<CommunicationView>(model);
        }
        public static Communication ToModel(this CommunicationView view)
        {
            return Mapper.Map<Communication>(view);
        }

        public static void SetTo(this CommunicationView view, Communication model)
        {
            Mapper.Map(view, model);
        }
        #endregion Communication

        #region Address

        public static AddressView ToView(this Address model)
        {
            return Mapper.Map<AddressView>(model);
        }
        public static Address ToModel(this AddressView view)
        {
            return Mapper.Map<Address>(view);
        }

        public static void SetTo(this AddressView view, Address model)
        {
            Mapper.Map(view, model);
        }
        #endregion Address

        #region Address

        public static SchoolAnnualAgreementView ToView(this SchoolAnnualAgreement model)
        {
            return Mapper.Map<SchoolAnnualAgreementView>(model);
        }
        public static SchoolAnnualAgreement ToModel(this SchoolAnnualAgreementView view)
        {
            return Mapper.Map<SchoolAnnualAgreement>(view);
        }

        public static void SetTo(this SchoolAnnualAgreementView view, SchoolAnnualAgreement model)
        {
            Mapper.Map(view, model);
        }
        #endregion Address

        #region Route

        public static SchoolRouteView ToView(this SchoolRoute model)
        {
            var view = Mapper.Map<SchoolRouteView>(model);
            return view;
        }
        public static SchoolRoute ToModel(this SchoolRouteView view)
        {
            return Mapper.Map<SchoolRoute>(view);
        }

        public static void SetTo(this SchoolRouteView view, SchoolRoute model)
        {
            Mapper.Map(view, model);
        }
        #endregion Address
    }
}
