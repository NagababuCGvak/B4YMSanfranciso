using Castle.MicroKernel;
using Tar.Core;
using NUnit.Framework;

namespace Tar.Tests.Core
{
    [TestFixture]
    public class ServiceLocationTestFixture
    {
        [Test]
        public void ResolveComponentWithoutKey()
        {
            var locator = ServiceLocator.Current;
            var obj = locator.Get<IInterface>();
            Assert.IsTrue(obj.WriteLine("test"));
        }

        [Test]
        public void ResolveComponentWithKey()
        {
            var locator = ServiceLocator.Current;
            var obj = locator.Get<IInterface>("i1");
            Assert.IsTrue(obj.WriteLine("test"));
        }

        [Test, ExpectedException(typeof(ComponentNotFoundException))]
        public void ThrowComponentNotFoundException ()
        {
            var locator = ServiceLocator.Current;
            var obj = locator.Get<IInterface>("i1071");
            Assert.IsTrue(obj.WriteLine("test"));
        }
    }
}
