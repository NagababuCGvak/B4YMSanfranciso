using Tar.Core;
using Tar.Repository.General.Multiple;

namespace Better4You.Core.Repositories
{
    public class ConfigRepository : MultipleRepository<IConfigEntity>, IConfigRepository
    {
        public ConfigRepository(IServiceLocator serviceLocator, string innerRepositoryKey)
            : base(serviceLocator, innerRepositoryKey)
        {
        }
    }
}