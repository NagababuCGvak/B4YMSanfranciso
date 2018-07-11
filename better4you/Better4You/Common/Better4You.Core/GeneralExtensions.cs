namespace Better4You.Core
{
    public static class GeneralExtensions
    {
        public static bool IsCompanyUser(this long userTypeId)
        {
            return userTypeId == (long)UserTypes.Company;
        }
        public static bool IsSchoolUser(this long userTypeId)
        {
            return userTypeId == (long)UserTypes.School;
        }
    }
}
