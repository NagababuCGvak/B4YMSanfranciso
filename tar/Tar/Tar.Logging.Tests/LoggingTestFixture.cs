using System.Diagnostics;
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
        public void LogDefault()
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
        [Test]
        public void LogDbLogger()
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
        public void LogEventLog()
        {
            var loggper = LoggingHelper.Instance.GetLogger("eventLog");
            loggper.Trace<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Trace" });
            loggper.Error<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Error" });
            loggper.Warn<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Warn" });
            loggper.Info<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Info" });
            loggper.Fatal<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Fatal" });
        }
        [Test]
        public void DebugViewEventLog()
        {
            var logger = LoggingHelper.Instance.GetLogger("debugViewLog");
            logger.Trace<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Trace" });
            logger.Error<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Error" });
            logger.Warn<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Warn" });
            logger.Info<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Info" });
            logger.Fatal<LoggingTestFixture>(new { Ad = "Reşad", Soyad = "Askerov", MessageType = "Fatal" });
        }
    }
}
