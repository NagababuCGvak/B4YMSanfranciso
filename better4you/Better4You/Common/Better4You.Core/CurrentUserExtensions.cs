using System;
using System.Collections.Generic;
using System.Linq;
using Tar.Security;

namespace Better4You.Core
{
    public static class CurrentUserExtensions
    {
        public static IList<GeneralItemView> Applications(this CurrentUser currentUser)
        {
            if (currentUser.Get<IList<GeneralItemView>>("Applications") == null)
                currentUser.Set("Applications", new List<GeneralItemView>());
            return currentUser.Get<IList<GeneralItemView>>("Applications");
        }

        public static void Applications(this CurrentUser currentUser, IList<GeneralItemView> applications)
        {
            currentUser.Set("Applications", applications);
        }

        public static List<Tar.ViewModel.GeneralItemView> Schools(this CurrentUser currentUser)
        {
            if (currentUser.Get<List<Tar.ViewModel.GeneralItemView>>("Schools") == null)
                currentUser.Set("Schools", new List<Tar.ViewModel.GeneralItemView>());

            return currentUser.Get<List<Tar.ViewModel.GeneralItemView>>("Schools");
        }

        public static void Schools(this CurrentUser currentUser, List<Tar.ViewModel.GeneralItemView> schools)
        {
            currentUser.Set("Schools", schools);
        }

        public static long UserTypeId(this CurrentUser currentUser)
        {
            return currentUser.Get<long>("UserTypeId");
        }
        public static string UserTypeName(this CurrentUser currentUser)
        {
            return ((UserTypes) currentUser.UserTypeId()).ToString("G");
        }
        public static CurrentUser UserTypeId(this CurrentUser currentUser, long userTypeId)
        {
            currentUser.Set("UserTypeId", userTypeId);
            return currentUser;
        }

        public static long CurrentApplicationId(this CurrentUser currentUser)
        {
            return currentUser.Get<long>("CurrentApplicationId");
        }

        public static CurrentUser CurrentApplicationId(this CurrentUser currentUser, long currentApplicationId)
        {
            currentUser.Set("CurrentApplicationId", currentApplicationId);
            return currentUser;
        }

        public static long CurrentSchoolId(this CurrentUser currentUser)
        {
            return currentUser.Get<long>("CurrentSchoolId");
        }

        public static CurrentUser CurrentSchoolId(this CurrentUser currentUser, long currentSchoolId)
        {
            currentUser.Set("CurrentSchoolId", currentSchoolId);
            return currentUser;
        }

        public static string CurrentSchoolName(this CurrentUser currentUser)
        {
            if (currentUser.UserTypeId().IsCompanyUser()) return "Company User";
            if (currentUser.CurrentSchoolId() == 0) return "School User";
            return currentUser.Schools()
                .First(x => x.Id.Equals(currentUser.CurrentSchoolId())).Text;
        }
    }
}