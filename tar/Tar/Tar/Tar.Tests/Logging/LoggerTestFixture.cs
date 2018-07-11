using System.Security.Principal;
using System.Threading;
using NUnit.Framework;
using Tar.Logging;

namespace Tar.Tests.Logging
{
    [TestFixture]
    public class LoggerTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            LoggingHelper.Initialize();
        }

        [Test]
        public void Test1()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("zahir"), new[] {"Admin"});
            this.LogInfo(new
                             {
                                 Prop1 = "Mehmet Zahir",
                                 Prop2 = "Solak",
                                 Prop3 = new {FirstName = "Reşad", LastName = "Askerov"},
                                 Ugur = "TEST"
                             }
                );
            M1();
        }

        private void M1()
        {
            using (this.CreateScope("M1"))
            {
                this.LogTrace("Trace");
                M2();
            }
        }

        private void M2()
        {
            using (this.CreateScope("M2"))
            {
                this.LogWarn("Warn");
                M3();
            }
        }

        private void M3()
        {
            using (this.CreateScope("M3"))
            {
                this.LogFatal("Fatal");
                M4();
            }
        }

        private void M4()
        {
            using (this.CreateScope("M4"))
            {
                this.LogError("Error");
                M5();
                
            }
        }
        private void M5()
        {
            using (this.CreateScope("M5"))
            {
                this.LogDebug("Debug");
            }
        }
    }
}