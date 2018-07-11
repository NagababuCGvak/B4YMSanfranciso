using System;
using NUnit.Framework;
using Tar.Core;
using Tar.Service.Messages;

namespace Tar.Tests.Service
{
    [TestFixture]
    public class ImplementationTestFixture
    {
        [Test]
        public void ServiceImpl()
        {
            ITest testService = new Test();
            Run(testService);
        }

        private void Run(ITest testService)
        {
            testService.Test1();
        }

        [Test]
        public void ServiceProxyImpl()
        {
            //ITest testService = new TestProxy("partner");
            //Run(testService);
        }

        [Test]
        public void Test1()
        {
            var response = new ServiceX {Locator = ServiceLocator.Current}.XMethod(new NullRequest());
            Assert.AreEqual(Result.Success, response.Result, response.Message);
        }
    }

    public interface IServiceX
    {
        Response XMethod(NullRequest request);
    }

    public class XResponse : Response
    {
    }

    public class ServiceX : Tar.Service.Service<IServiceX,ServiceX >, IServiceX
    {
        public Response XMethod(NullRequest request)
        {
            return Execute<NullRequest, XResponse>(request, response =>
                                                            {
                                                                Test1();

                                                            });
        }

        private void Test1()
        {
            try
            {
                Test2();
            }
            catch (Exception exception)
            {
                throw new Exception("Test1.Exception", exception);
            }
        }

        private void Test2()
        {
            try
            {
                Test3();
            }
            catch (Exception exception)
            {
                throw new Exception("Test2.Exception", exception);
            }
        }
        private void Test3()
        {
            throw new Exception("Test3.Exception");
        }
    }
}
