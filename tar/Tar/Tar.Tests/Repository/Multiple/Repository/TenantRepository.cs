using Tar.Core;
using Tar.Repository.General.Multiple;

namespace Tar.Tests.Repository.Multiple.Repository
{
    public class TenantRepository : MultipleRepository<ITenantEntity>
    {
        public TenantRepository(IServiceLocator serviceLocator, string innerRepositoryKey)
            : base(serviceLocator, innerRepositoryKey)
        {
        }
    }
}