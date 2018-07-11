using Tar.Core;
using Tar.Repository.General;
using Tar.Tests.Model;
using NUnit.Framework;

namespace Tar.Tests.Repository
{
    //[TestFixture]
    public class NhMappingTestFixture
    {
        private IServiceLocator _serviceLocator;
        private IGeneralRepository _repository;

        [SetUp]
        public void SetUp()
        {
            ServiceLocator.Current = new WindsorServiceLocator();
            _serviceLocator = ServiceLocator.Current;

            //var nhc = _serviceLocator.Get<INhDatabaseConfiguration>();

            //var dbContext = _serviceLocator.Get<EfDbContext>();
            ////dbContext.Database.Delete();
            ////dbContext.Database.Create(); 

            _repository = _serviceLocator.Get<IGeneralRepository>();
            //TODO: USE MOCK
            _repository.DeleteAll<Product>();
            _repository.DeleteAll<Category>();
        }
        /*
        [Test ,Category("NhMapping")]
        public void MappingByCode()
        {
            //Windsor.Repository.Nh
            //Add entry node with key attribute value Code 
            _repository.Create(new Category {Name = Guid.NewGuid().ToString()});
            IDictionary<MappingType, IEnumerable<string>> mappings =
                new Dictionary<MappingType, IEnumerable<string>>
                    {
                        {MappingType.Xml, new[] {"Tar.Tests"}},
                        {MappingType.Code, new[] {"Tar.Tests"}}
                    };

            string scriptFolder = "";
            INhDatabaseConfiguration nhdb = new NhDatabaseConfiguration(mappings, scriptFolder);
            INhDatabaseConfiguration nhc = _serviceLocator.Get<INhDatabaseConfiguration>();

            var mock = new Mock<INhDatabaseConfiguration>();
            mock.Setup(foo => foo.GetMappings(MappingType.Code)).Returns(new[] { "Tar.Tests" });
            mock.Setup(foo => foo.GetMappings(MappingType.Xml)).Returns(new[] { "Tar.Tests" });

            var nhconfiguration = new NhRepositoryConfiguration(mock.Object);
            ISession session = nhconfiguration.GetSessionFactory().OpenSession();
        }
        */
    }
}
