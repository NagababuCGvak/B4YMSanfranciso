using Tar.Core;
using Tar.Repository.General.Multiple;

namespace Tar.Tests.Repository.Multiple.Repository
{
    public class ConfigRepository : MultipleRepository<IConfigEntity>
    {
        public ConfigRepository(IServiceLocator serviceLocator, string innerRepositoryKey)
            : base(serviceLocator, innerRepositoryKey)
        {
        }
    }
}