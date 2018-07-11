using System;
using System.Linq;
using NUnit.Framework;
using Tar.Core;
using Tar.Tests.Repository.Multiple.Model;
using Tar.Tests.Repository.Multiple.Repository;

namespace Tar.Tests.Repository.Multiple
{
    [TestFixture]
    public class ConfigRepositoryTestCase
    {
        private IServiceLocator _locator;
        [SetUp]
        public void SetUp()
        {
            const string configFilePath = @"Repository\Multiple\ConfigRepository.config";
            ServiceLocator.Initialize(configFilePath);
            _locator = ServiceLocator.Current;
        }

        [Test]
        public void CreateUsers()
        {
            var configRepo = _locator.Get<ConfigRepository>();
            CreateRandomUsers(configRepo, 5);
        }

        public static void CreateRandomUsers(ConfigRepository configRepo, int count)
        {
            Enumerable.Range(0, count)
                .ToList().ForEach(
                    i => configRepo.Create(
                        new User
                            {
                                UserName = "zahir-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5),
                                Password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5)
                            }));
            Console.WriteLine("Created {0} users.", count);
        }
    }
}
