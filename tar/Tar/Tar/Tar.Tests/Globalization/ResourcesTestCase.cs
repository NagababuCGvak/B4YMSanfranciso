using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using Tar.Globalization;

namespace Tar.Tests.Globalization
{
    [TestFixture]
    public class ResourcesTestCase
    {
        [SetUp]
        public void SetUp()
        {
            Resources.SetResourceRepository(new DbResourceRepository("DefaultConnection"));
        }

        [Test]
        public void FieldTestsOnOther()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fr-FR");
            var field = Resources.Field("RESOURCEID");
            Assert.AreEqual(field, "Resource No-default");
        }

        [Test]
        public void FieldTestsOnEnUs()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var field = Resources.Field("RESOURCEID");
            Assert.AreEqual(field, "Resource Number");
        }

        [Test]
        public void FieldTestsOnTrTr()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");
            var field = Resources.Field("RESOURCEID");
            Assert.AreEqual(field, "Resource No");
        }
    }
}
