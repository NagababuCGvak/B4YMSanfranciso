using System;
using System.IO;
using NUnit.Framework;
using Tar.Service;
using Tar.Service.Messages;

namespace Tar.Logging.Tests
{
    [TestFixture]
    public class Bug387
    {
        [Test]
        public void Test()
        {
            LoggingHelper.Initialize();
            using (this.CreateScope("Bug387"))
            {
                //this.LogInfo(new { FileInfo = new FileInfo(@"C:\src\logper\Tar\Tar\SharedLibs\NVelocity\NVelocity.dll") });
                //this.LogInfo(new { Message = "Test Message" });
                this.LogInfo(new { Exception = new Exception("TEST EXCEPTION-1") });
                this.LogInfo(new Exception("TEST EXCEPTION-2"));
                this.LogInfo(new TarServiceOnErrorEventArgs(new PageableRequest(), new PageableResponse(),
                    new Exception("TEST EXCEPTION-3")));
                this.LogInfo(
                    new
                    {
                        NVelocity_222KB = File.ReadAllBytes(GetFilePath(@"SharedLibs\NVelocity\NVelocity.dll")),
                        NHibernateDll_3_39MB = File.ReadAllBytes(GetFilePath(@"SharedLibs\NHibernate\NHibernate.dll")),
                        NHibernateXml_2_24MB = File.ReadAllBytes(GetFilePath(@"SharedLibs\NHibernate\NHibernate.xml")),
                        CastleWindsorDll_346KB =
                            File.ReadAllBytes(GetFilePath(@"SharedLibs\Castle.Windsor\Castle.Windsor.dll")),
                    });
            }
        }

        private string GetFilePath(string filePath)
        {
            return string.Format(@"..\..\..\..\{0}", filePath);
        }
    }
}
