using NUnit.Framework;
using Tar.Core;
using Tar.Repository;
using Tar.Tests.Repository.Multiple.Repository;

namespace Tar.Tests.Repository.Multiple
{
    [TestFixture]
    public class MultipleRepositoryTestCase
    {
        private IServiceLocator _locator;
        [SetUp]
        public void SetUp()
        {
            const string configFilePath = @"Repository\Multiple\MultipleRepository.config";
            ServiceLocator.Initialize(configFilePath);
            _locator = ServiceLocator.Current;
        }

        [Test]
        public void CreateUsersAndAccounts()
        {
            var configRepo = _locator.Get<ConfigRepository>();
            ConfigRepositoryTestCase.CreateRandomUsers(configRepo, 5);
            var tenantRepo = _locator.Get<TenantRepository>();
            TenantRepositoryTestCase.CreateRandomAccounts(tenantRepo, 5);
        }

        [Test]
        public void CreateUsersAndAccountsInUnitOfWork()
        {
            using (var uow = _locator.Get<IUnitOfWork>())
            {
                var configRepo = _locator.Get<ConfigRepository>();
                ConfigRepositoryTestCase.CreateRandomUsers(configRepo, 5);
                var tenantRepo = _locator.Get<TenantRepository>();
                TenantRepositoryTestCase.CreateRandomAccounts(tenantRepo, 5);
                uow.Commit();
            }
        }
    }
}