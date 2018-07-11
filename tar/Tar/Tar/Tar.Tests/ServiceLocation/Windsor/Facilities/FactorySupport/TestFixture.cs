using Tar.Core;
using NUnit.Framework;

namespace Tar.Tests.ServiceLocation.Windsor.Facilities.FactorySupport
{
    [TestFixture]
    public class FactorySupportTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            ServiceLocator.Reset();
            const string configFilePath = @"ServiceLocation\Windsor\Facilities\FactorySupport\"
                                          + @"Windsor.FactoryMethod.config";
            //var type = Type.GetType("Castle.Facilities.FactorySupport.FactorySupportFacility, "
            //                 + "Castle.Facilities.FactorySupport");
            ServiceLocator.Initialize(configFilePath);
        }

        [Test]
        public void ResolveService()
        {
            var service = ServiceLocator.Current.Get<IService>();
            Assert.That(service, Is.Not.Null);
            Assert.That(service.Component, Is.Not.Null);
            Assert.That(service.Component.Name, Is.Not.Null);
            Assert.That(service.Component.Name, Is.EqualTo("Zahir"));
        }
    }
}
