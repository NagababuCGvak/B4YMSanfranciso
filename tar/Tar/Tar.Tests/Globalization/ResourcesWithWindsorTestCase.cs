using System.Globalization;
using System.Threading;
using NUnit.Framework;
using Tar.Core;
using Tar.Globalization;

namespace Tar.Tests.Globalization
{
    [TestFixture]
    public class ResourcesWithWindsorTestCase
    {
        private IResourceManager _resourceManager;

        [SetUp]
        public void SetUp()
        {
            const string configFilePath = @"Globalization\Windsor.config";
            ServiceLocator.Initialize(configFilePath);
            _resourceManager = ServiceLocator.Current.Get<IResourceManager>();
        }

        [Test]
        public void FieldTestsOnOther()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fr-FR");
            var field = _resourceManager.Field("RESOURCEID");
            Assert.AreEqual(field, "Resource No-default");
        }

        [Test]
        public void FieldTestsOnEnUs()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var field = _resourceManager.Field("RESOURCEID");
            Assert.AreEqual(field, "Resource Number");
        }

        [Test]
        public void FieldTestsOnTrTr()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");
            var field = _resourceManager.Field("RESOURCEID");
            Assert.AreEqual(field, "Resource No");
        }
    }
}