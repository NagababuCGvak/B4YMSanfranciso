using System;
using System.Linq;
using Tar.Core;
using Tar.Core.Configuration;
using Tar.Repository;
using Tar.Repository.General;
using Tar.Tests.Model;
using NUnit.Framework;

namespace Tar.Tests.Repository
{
    [TestFixture]
    public class RepositoryTestFixture
    {
        private IServiceLocator _serviceLocator;
        private ITestRepository<Product> _repository;
        private IGeneralRepository _generalRepository;
        private IConnectionStringsSettings _connectionStringsSettings;

        [SetUp]
        public void SetUp()
        {
            ServiceLocator.Reset();
            _serviceLocator = ServiceLocator.Current;
            _repository = _serviceLocator.Get<ITestRepository<Product>>();
            _connectionStringsSettings = _serviceLocator.Get<IConnectionStringsSettings>();
            _generalRepository = _serviceLocator.Get<IGeneralRepository>();
        }
        [TearDown]
        public void TearDown()
        {
            //_repository.DeleteAll();
        }

        [Test, Category("Repository")]
        public void EfTest()
        {
            var efRepo = _serviceLocator.Get<IRepository<Product>>("ef");

            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product {Name = "ADSL Modem"};
                Assert.AreEqual(product.Id, 0);
                efRepo.Create(product);
                Assert.AreNotEqual(product.Id, 0);
                uow.Commit();
            }


            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product {Name = "ADSL Modem"};
                Assert.AreEqual(product.Id, 0);
                efRepo.Create(product);
                Assert.AreNotEqual(product.Id, 0);
                uow.Commit();
            }


            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product {Name = "ADSL Modem"};
                Assert.AreEqual(product.Id, 0);
                efRepo.Create(product);
                Assert.AreNotEqual(product.Id, 0);


                using (var uow2 = _serviceLocator.Get<IUnitOfWork>())
                {
                    var product2 = new Product {Name = "ADSL Modem2"};
                    Assert.AreEqual(product2.Id, 0);
                    efRepo.Create(product2);
                    Assert.AreNotEqual(product2.Id, 0);
                    uow2.Commit();
                }
                uow.Commit();
            }
        }

        [Test, Category("Repository")]
        public void NhTest()
        {
            var efRepo = _serviceLocator.Get<IRepository<Product>>("DefaultConnection.nh");

            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product { Name = "ADSL Modem" };
                Assert.AreEqual(product.Id, 0);
                efRepo.Create(product);
                Assert.AreNotEqual(product.Id, 0);
                uow.Commit();
            }


            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product { Name = "ADSL Modem" };
                Assert.AreEqual(product.Id, 0);
                efRepo.Create(product);
                Assert.AreNotEqual(product.Id, 0);
                uow.Commit();
            }


            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product { Name = "ADSL Modem" };
                Assert.AreEqual(product.Id, 0);
                efRepo.Create(product);
                Assert.AreNotEqual(product.Id, 0);


                using (var uow2 = _serviceLocator.Get<IUnitOfWork>())
                {
                    var product2 = new Product { Name = "ADSL Modem2" };
                    Assert.AreEqual(product2.Id, 0);
                    efRepo.Create(product2);
                    Assert.AreNotEqual(product2.Id, 0);
                    uow2.Commit();
                }
                uow.Commit();
            }
        }

        [Test, Category("Repository")]
        public void CreateProductWithoutUnitOfWork()
        {
            var product = new Product {Name = "ADSL Modem"};
            _repository.Create(product);
            Assert.AreNotEqual(product.Id, 0);
            var productCount = _repository.Query().Count();
            Assert.That(productCount, Is.EqualTo(1));
            var productFromDb = _repository.GetById(product.Id);
            Assert.That(productFromDb, Is.Not.Null);
            Assert.That(productFromDb.Id, Is.EqualTo(product.Id));
            Assert.That(productFromDb.Name, Is.EqualTo(product.Name));
        }

        [Test, Category("Repository")]
        public void CreateProductWithUnitOfWork()
        {
            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product {Name = "ADSL Modem"};
                Assert.AreEqual(product.Id, 0);
                _repository.Create(product);
                Assert.AreNotEqual(product.Id, 0);
                uow.Commit();
            }

            var productCount = _repository.Query().Count();
            Assert.That(productCount, Is.EqualTo(1));
        }

        [Test, Category("Repository")]
        public void CreateProductWithDoubleUnitOfWork()
        {
            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product {Name = "ADSL Modem 1"};
                Assert.AreEqual(product.Id, 0);
                _repository.Create(product);
                Assert.AreNotEqual(product.Id, 0);
                uow.Commit();
            }
            var productCount = _repository.Query().Count();
            Assert.That(productCount, Is.EqualTo(1));

            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product {Name = "ADSL Modem 2"};
                Assert.AreEqual(product.Id, 0);
                _repository.Create(product);
                Assert.AreNotEqual(product.Id, 0);
                uow.Commit();
            }

            productCount = _repository.Query().Count();
            Assert.That(productCount, Is.EqualTo(2));
        }

        [Test, Category("Repository")]
        public void CreateProductWithNestedUnitOfWork()
        {
            Assert.AreEqual(_repository.Query().Count(), 0);
            using (var uow = _serviceLocator.Get<IUnitOfWork>())
            {
                var product = new Product {Name = "ADSL Modem 1"};
                Assert.AreEqual(product.Id, 0);
                _repository.Create(product);
                Assert.AreNotEqual(product.Id, 0);

                Assert.AreEqual(_repository.Query().Count(), 1, "Kayıt sayısı 1 olmalı.");

                using (var uow2 = _serviceLocator.Get<IUnitOfWork>())
                {
                    var product2 = new Product {Name = "ADSL Modem 2"};
                    Assert.AreEqual(product2.Id, 0);
                    _repository.Create(product2);
                    Assert.AreNotEqual(product2.Id, 0);

                    Assert.AreEqual(_repository.Query().Count(), 2, "Kayıt sayısı 2 olmalı.");
                    uow2.Commit();
                }
                Assert.AreEqual(_repository.Query().Count(), 2, "Kayıt sayısı 2 olmalı.");
                uow.Commit();
            }
            Assert.AreEqual(_repository.Query().Count(), 2, "Kayıt sayısı 2 olmalı.");
        }

        [Test]
        public void ConnectionStringEncryDecrypt()
        {
            const string plainText = @"Server=.\SQL2012;Initial Catalog=master;User Id=logper;Password=logper;";
            var encrypted = _connectionStringsSettings.Encrypt(plainText);
            Assert.AreNotEqual(encrypted, plainText);
            var decrypted = _connectionStringsSettings.Decrypt(encrypted);
            Assert.AreNotEqual(decrypted, encrypted);
            Assert.AreEqual(plainText, decrypted);
            Console.WriteLine(encrypted);
        }
    }
}
