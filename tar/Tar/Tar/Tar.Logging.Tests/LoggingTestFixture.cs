using NUnit.Framework;

namespace Tar.Logging.Tests
{
    [TestFixture]
    public class LoggingTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            LoggingHelper.Initialize();
        }

        [Test]
        public void Log1()
        {
            var logger = LoggingHelper.Instance.GetLogger();
            logger.Info<LoggingTestFixture>(new
            {
                Ad = "Mehmet Zahir",
                Soyad = "Solak"
            });
            var dbLogger = LoggingHelper.Instance.GetLogger("dblogger");
            dbLogger.Trace<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov" });
        }

        [Test]
        public void Log2()
        {
            this.LogInfo(new
            {
                Ad = "Mehmet Zahir",
                Soyad = "Solak"
            });

            this.LogTrace(new
                              {
                                  Ad = "Reşad",
                                  Soyad = "Askerov"
                              });
        }
    }
}
