using NUnit.Framework;

namespace Tar.Tests.Core
{
    [TestFixture]
    public class FactoryMethodTestFixture
    {
        [Test]
        public void One()
        {
            //var serviceProxyCreator = new ServiceProxyCreator<ITestLoggerService>();
        }
    }

    public interface IServiceProxyCreator<out T>
    {
        T CreateService();
    }

    public class ServiceProxyCreator<T> : IServiceProxyCreator<T>
    {
        public T CreateService()
        {
            throw new System.NotImplementedException();
        }
    }
}
