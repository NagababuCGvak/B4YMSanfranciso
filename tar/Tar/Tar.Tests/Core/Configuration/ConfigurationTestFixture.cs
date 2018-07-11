using NUnit.Framework;
using Tar.Core;
using Tar.Core.Configuration;
using System;

namespace Tar.Tests.Core.Configuration
{
    [TestFixture]
    public class ConfigurationTestFixture
    {
        private IServiceLocator _serviceLocator;
        [SetUp]
        public void SetUp()
        {
            ServiceLocator.Reset();
            _serviceLocator = ServiceLocator.Current;
        }

        [Test]
        public void GetIApplicationSettingsComponent()
        {
            var applicationSettings = _serviceLocator.Get<ITestSettings>();
            Assert.That(applicationSettings, Is.Not.Null);

            var isOnline = applicationSettings.IsOnline;
            Assert.That(isOnline, Is.True);
        }


        [Test]
        public void GetISettingsComponent()
        {
            var settings = _serviceLocator.Get<ISettings>("Settings.Application");
            Assert.That(settings, Is.Not.Null);

            var isOnline = settings.GetSetting<bool>("IsOnline");
            Assert.That(isOnline, Is.True);

            var guid = settings.GetSetting<Guid>("Guid");
            Assert.That(guid, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void AreSameSettingsAndTestSettings()
        {
            var settings = _serviceLocator.Get<ISettings>("Settings.Application");
            var testSettings = _serviceLocator.Get<ITestSettings>();

            Assert.That(settings.GetSetting<bool>("IsOnline"), Is.EqualTo(testSettings.IsOnline));
            Assert.That(settings.GetSetting<int>("Integer"), Is.EqualTo(testSettings.Integer));
            Assert.That(settings.GetSetting<Guid>("Guid"), Is.EqualTo(testSettings.Guid));

            Assert.That(settings.GetSetting<bool>("IsOnline"), Is.EqualTo(testSettings.IsOnline));
            Assert.That(settings.GetSetting<int>("Integer"), Is.EqualTo(testSettings.Integer));
            Assert.That(settings.GetSetting<Guid>("Guid"), Is.EqualTo(testSettings.Guid));

            Assert.That(settings.GetSetting<bool>("IsOnline"), Is.EqualTo(testSettings.IsOnline));
            Assert.That(settings.GetSetting<int>("Integer"), Is.EqualTo(testSettings.Integer));
            Assert.That(settings.GetSetting<Guid>("Guid"), Is.EqualTo(testSettings.Guid));

            Assert.That(settings.GetSetting<bool>("IsOnline"), Is.EqualTo(testSettings.IsOnline));
            Assert.That(settings.GetSetting<int>("Integer"), Is.EqualTo(testSettings.Integer));
            Assert.That(settings.GetSetting<Guid>("Guid"), Is.EqualTo(testSettings.Guid));

            Assert.That(settings.GetSetting<bool>("IsOnline"), Is.EqualTo(testSettings.IsOnline));
            Assert.That(settings.GetSetting<int>("Integer"), Is.EqualTo(testSettings.Integer));
            Assert.That(settings.GetSetting<Guid>("Guid"), Is.EqualTo(testSettings.Guid));
        }
    }

}
