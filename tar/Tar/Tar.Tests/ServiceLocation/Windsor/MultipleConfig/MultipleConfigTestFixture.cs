using System;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using NUnit.Framework;
using Tar.Core;

namespace Tar.Tests.ServiceLocation.Windsor.MultipleConfig
{
    [TestFixture]
    public class MultipleConfigTestFixture
    {
        const string Config1FilePath = @"ServiceLocation\Windsor\MultipleConfig\Windsor.Config1.config";
        const string Config2FilePath = @"ServiceLocation\Windsor\MultipleConfig\Windsor.Config2.config";
        const string Config3FilePath = @"ServiceLocation\Windsor\MultipleConfig\Windsor.Config3.config";

        [Test]
        public void WithWindsorContainer()
        {
            var windsorContainer = new WindsorContainer(new XmlInterpreter(Config1FilePath));
            var component = windsorContainer.Resolve<MultipleConfigComponent>();
            Assert.IsNotNull(component);
            Assert.AreEqual("config1", component.Name);

            component.Name = Guid.NewGuid().ToString();

            var components = windsorContainer.ResolveAll<MultipleConfigComponent>();
            Assert.AreEqual(1, components.Length);


            windsorContainer.Install(new ConfigurationInstaller(new XmlInterpreter(Config2FilePath)));

            components = windsorContainer.ResolveAll<MultipleConfigComponent>();
            Assert.AreEqual(2, components.Length);

            windsorContainer.Install(new ConfigurationInstaller(new XmlInterpreter(Config3FilePath)));

            components = windsorContainer.ResolveAll<MultipleConfigComponent>();
            Assert.AreEqual(3, components.Length);

            var component1 = windsorContainer.Resolve<MultipleConfigComponent>("config1");
            Assert.IsNotNull(component1);
            Assert.AreEqual(component.Name, component1.Name);
            Assert.AreEqual(component, component1);

            var component2 = windsorContainer.Resolve<MultipleConfigComponent>("config2");
            Assert.IsNotNull(component2);
            Assert.AreEqual("config2", component2.Name);
            Assert.AreNotEqual(component, component2);
            Assert.AreNotEqual(component1, component2);
        }

        [Test]
        public void WithServiceLocator()
        {
            ServiceLocator.Initialize(Config1FilePath);
            var serviceLocator = ServiceLocator.Current;

            var component = serviceLocator.Get<MultipleConfigComponent>();
            Assert.IsNotNull(component);
            Assert.AreEqual("config1", component.Name);

            component.Name = Guid.NewGuid().ToString();

            var components = serviceLocator.GetAll<MultipleConfigComponent>();
            Assert.AreEqual(1, components.Length);


            serviceLocator.AddConfig(Config2FilePath);

            components = serviceLocator.GetAll<MultipleConfigComponent>();
            Assert.AreEqual(2, components.Length);

            serviceLocator.AddConfig(Config3FilePath);

            components = serviceLocator.GetAll<MultipleConfigComponent>();
            Assert.AreEqual(3, components.Length);

            var component1 = serviceLocator.Get<MultipleConfigComponent>("config1");
            Assert.IsNotNull(component1);
            Assert.AreEqual(component.Name, component1.Name);
            Assert.AreEqual(component, component1);

            var component2 = serviceLocator.Get<MultipleConfigComponent>("config2");
            Assert.IsNotNull(component2);
            Assert.AreEqual("config2", component2.Name);
            Assert.AreNotEqual(component, component2);
            Assert.AreNotEqual(component1, component2);
        }
    }
}
