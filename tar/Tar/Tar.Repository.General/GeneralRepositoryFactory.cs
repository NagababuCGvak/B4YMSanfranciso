using Tar.Core;

namespace Tar.Repository.General
{
    public static class GeneralRepositoryFactory
    {
        public static T Get<T>(string key = null)
            where T : IGeneralRepository
        {
            return string.IsNullOrEmpty(key)
                       ? ServiceLocator.Current.Get<T>()
                       : ServiceLocator.Current.Get<T>(key);
        }
    }
}