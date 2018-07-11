using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using Tar.Globalization;
using System.Data.Common;
using System.Data;

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
        public void ReadAllDataProviders()
        { var dataTable = DbProviderFactories.GetFactoryClasses();
            foreach (DataRow item in dataTable.Rows)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    Console.WriteLine("{0}:{1}", column.ColumnName, item[column.ColumnName]);
                }

                Console.WriteLine("---------------------------------------------------");
            }
        }

        [Test]
        public void FieldTestsOnOther()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fr-FR");
            var field = Resources.Field("RESOURCEID");
            Assert.AreEqual("Resource No-default", field);
        }

        [Test]
        public void FieldTestsOnEnUs()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var field = Resources.Field("RESOURCEID");
            Assert.AreEqual("Resource Number", field);
        }

        [Test]
        public void FieldTestsOnTrTr()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");
            var field = Resources.Field("RESOURCEID");
            Assert.AreEqual("Resource No",field);
        }
    }
}
