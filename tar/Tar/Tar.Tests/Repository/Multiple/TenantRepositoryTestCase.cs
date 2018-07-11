using System;
using System.Linq;
using NUnit.Framework;
using Tar.Core;
using Tar.Tests.Repository.Multiple.Model;
using Tar.Tests.Repository.Multiple.Repository;

namespace Tar.Tests.Repository.Multiple
{
    [TestFixture]
    public class TenantRepositoryTestCase
    {
        private IServiceLocator _locator;
        [SetUp]
        public void SetUp()
        {
            const string configFilePath = @"Repository\Multiple\TenantRepository.config";
            ServiceLocator.Initialize(configFilePath);
            _locator = ServiceLocator.Current;
        }

        [Test]
        public void CreateAccounts()
        {
            var tenantRepo = _locator.Get<TenantRepository>();
            CreateRandomAccounts(tenantRepo, 5);
        }

        public static void CreateRandomAccounts(TenantRepository tenantRepo, int count)
        {
            Enumerable.Range(0, count)
                .ToList().ForEach(
                    i => tenantRepo.Create(
                        new Account
                            {
                                Code = "code-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5),
                                Name = "name-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5)
                            }));
            Console.WriteLine("Created {0} accounts.", count);
        }
    }
}